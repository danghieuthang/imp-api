using IMP.Application.Models;
using IMP.Application.Models.Account;
using IMP.Application.Models.Email;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Helpers;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Wrappers;
using IMP.Domain.Settings;
using IMP.Infrastructure.Identity.Helpers;
using IMP.Infrastructure.Identity.Models;
using IMP.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using IMP.Application.Constants;
using IMP.Infrastructure.Identity.Contexts;

namespace IMP.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IGoogleService _googleServices;
        private readonly IFacebookService _facebookService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork<IdentityContext> _unitOfWork;
        public AccountService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<User> signInManager,
            IEmailService emailService,
            IApplicationUserService applicationUserService,
            IGoogleService googleServices,
            IFacebookService facebookService,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork<IdentityContext> unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _emailService = emailService;
            _applicationUserService = applicationUserService;
            _googleServices = googleServices;
            _facebookService = facebookService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.Email);
                if (user == null)
                    throw new ValidationException(new ValidationError("email", $"No Accounts Registered with {request.Email}."), code: ErrorConstants.Identity.EmailNotFound);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ValidationException(new ValidationError("password", $"Invalid Credentials for '{request.Email}'."), code: ErrorConstants.Identity.PasswordIncorrect);
            }
            if (!user.EmailConfirmed)
            {
                throw new ValidationException(new ValidationError("email", $"Account Not Confirmed for '{request.Email}'."), code: ErrorConstants.Identity.EmailNotConfirm);
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;

            var refreshTokenDomain = new RefreshToken
            {
                Token = refreshToken.Token,
                Created = refreshToken.Created,
                Expires = refreshToken.Expires,
                CreatedByIp = ipAddress,
                UserId = user.Id
            };
            await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenDomain);
            await _unitOfWork.CommitAsync();
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        private async Task<string> GenerateUnDuplicateUserName(string username)
        {
            username = username.ConvertToUnSign().ToLower().Replace(" ", "");
            var userWithSameUserName = await _userManager.FindByNameAsync(username);
            int i = 1;
            while (userWithSameUserName != null)
            {
                i++;
                if (i == 2)
                {
                    username += i;
                }
                else
                {
                    username = username.Substring(0, username.Length - 1) + i;
                }
                userWithSameUserName = await _userManager.FindByNameAsync(username);
            }
            return username;
        }
        private async Task<User> RegisterSocialAsync(ProviderUserDetail providerUser, RegisterRole role = RegisterRole.Influencer)
        {
            //string username = await GenerateUnDuplicateUserName(providerUser.Name);

            var user = new User
            {
                Email = providerUser.Email,
                UserName = providerUser.ProviderUserId,
                FirstName = providerUser.FirstName,
                LastName = providerUser.LastName,
                EmailConfirmed = true,
                ProviderUserId = providerUser.ProviderUserId,
                IsChangeUsername = false,
            };
            var userWithProviderId = _userManager.Users?.FirstOrDefault(x => x.ProviderUserId == user.ProviderUserId);
            if (userWithProviderId == null)
            {


                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role.ToString());
                    // Create Application User
                    var applicationUser = await _applicationUserService.CreateUser(userRole: role, avatar: providerUser.Avatar);
                    // Add Application User ref to Identity User
                    if (applicationUser != null)
                    {
                        user.ApplicationUserId = applicationUser.Id;
                        await _userManager.UpdateAsync(user);
                    }
                    return user;
                }
            }

            throw new ApiException($"This account is already registered.");

        }
        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {

            var userWithSameUserName = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameUserName != null && userWithSameUserName.EmailConfirmed)
            {
                var error = new ValidationError("email", $"Email '{request.Email}' is already taken.");
                throw new ValidationException(error);
            }else if (userWithSameUserName != null)
            {
               await _userManager.DeleteAsync(userWithSameUserName);
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                IsChangeUsername = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                // Create Application User
                var applicationUser = await _applicationUserService.CreateUser(userRole: request.Role, user.Email);
                // Add Application User ref to Identity User
                if (applicationUser != null)
                {
                    user.ApplicationUserId = applicationUser.Id;
                    user.EmailConfirmed = false;
                    await _userManager.UpdateAsync(user);
                }
                await _userManager.AddToRoleAsync(user, request.Role.ToString());
                var verificationUri = await SendVerificationEmail(user, origin);
                //TODO: Attach Email Service here and configure it via appsettings
                await _emailService.SendAsync(new Application.Models.Email.EmailRequest() { To = user.Email, Body = $"Please confirm your account by click <a href='{verificationUri}'>Here</a>.", Subject = "Confirm Registration TMP Platform" });
                return new Response<string>(user.Email, message: $"User Registered.");
            }
            var errors = new List<ValidationError>();
            foreach (var error in result.Errors)
            {
                errors.Add(new ValidationError("passsword", error.Description));
            }

            throw new ValidationException(errors);

        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("appid", user.ApplicationUserId.GetValueOrDefault().ToString()),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(User user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string domain = _httpContextAccessor.HttpContext.Request.Host.Value;
            if (string.IsNullOrEmpty(domain))
            {
                domain = "http://localhost";
            }
            else
            {
                domain = "https://" + domain;
            }
            var route = "/api/accounts/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{domain}", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Response<string>(new ValidationError("userId", "Account not exists."));
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    result = await _userManager.RemoveFromRolesAsync(user, roles);
                }
                if (result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                }
                return new Response<string>(new ValidationError("code", "Invalid code."));
            }
        }

        private RefreshTokenResponse GenerateRefreshToken(string ipAddress)
        {
            return new RefreshTokenResponse
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";

            string domain = _httpContextAccessor.HttpContext.Request.Host.Value;
            if (string.IsNullOrEmpty(domain))
            {
                domain = "http://localhost";
            }
            else
            {
                domain = "https://" + domain;
            }

            var _enpointUri = new Uri(string.Concat($"{domain}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }

        public async Task<Response<AuthenticationResponse>> RefreshToken(string refreshToken, string ipAddress)
        {
            var repository = _unitOfWork.Repository<RefreshToken>();
            var entity = await repository.FindSingleAsync(x => x.Token == refreshToken, includeProperties: x => x.User);
            if (entity == null) throw new ValidationException(new ValidationError("refresh_token", "Token not exist."), ErrorConstants.RefreshToken.RefreshTokenNotExist); ;

            if (!entity.IsActive)
            {
                repository.Delete(entity);
                await _unitOfWork.CommitAsync();
                var error = new ValidationError("refresh_token", "Token was expired.");
                throw new ValidationException(error, ErrorConstants.RefreshToken.RefreshTokenWasExpired);
            }

            var user = entity.User;

            var newJwtToken = await GenerateJWToken(entity.User);
            var newRefreshToken = GenerateRefreshToken(ipAddress);


            #region update refresh token after refresh
            entity.Revoked = DateTime.UtcNow;
            entity.RevokedByIp = ipAddress;
            entity.ReplacedByToken = newRefreshToken.Token;
            repository.Update(entity);
            await _unitOfWork.CommitAsync();
            #endregion

            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(newJwtToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.RefreshToken = newRefreshToken.Token;

            var refreshTokenDomain = new RefreshToken
            {
                Token = newRefreshToken.Token,
                Created = newRefreshToken.Created,
                Expires = newRefreshToken.Expires,
                CreatedByIp = ipAddress,
                UserId = user.Id
            };
            return new Response<AuthenticationResponse>(response);
        }
        private async Task<RefreshToken> GetRefreshToken(string refreshToken, string ipAddress)
        {
            return await _unitOfWork.Repository<RefreshToken>().FindSingleAsync(x => x.Token == refreshToken, includeProperties: x => x.User);
        }
        public async Task<Response<string>> RevokeToken(string refreshToken, string ipaAddress)
        {
            var entity = await GetRefreshToken(refreshToken, ipaAddress);
            if (entity == null)
            {
                var error = new ValidationError("refresh_token", "Refresh Token not exist.");
                throw new ValidationException(error);
            }

            if (!entity.IsActive)
            {
                var error = new ValidationError("refresh_token", "Refresh Token was revoked.");
                throw new ValidationException(error);
            }

            entity.Revoked = DateTime.UtcNow;
            entity.RevokedByIp = ipaAddress;
            _unitOfWork.Repository<RefreshToken>().Update(entity);
            await _unitOfWork.CommitAsync();
            return new Response<string>(entity.User.Email, $"Refresh token was revoked.");
        }

        private async Task<AuthenticationResponse> AuthenticationWithoutPassword(string ipAddress, string username = null, string email = null, string providerId = null)
        {
            User user;
            if (providerId != null)
            {
                user = _userManager.Users?.FirstOrDefault(x => x.ProviderUserId == providerId);
                if (user == null)
                {
                    throw new ApiException($"No Accounts Registered with {providerId}.");
                }
            }
            else if (username != null)
            {
                user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new ApiException($"No Accounts Registered with {username}.");
                }

            }
            else
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new ApiException($"No Accounts Registered with {email}.");
                }
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;

            var refreshTokenDomain = new RefreshToken
            {
                Token = refreshToken.Token,
                Created = refreshToken.Created,
                Expires = refreshToken.Expires,
                CreatedByIp = ipAddress,
                UserId = user.Id
            };
            _ = await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenDomain);
            await _unitOfWork.CommitAsync();
            return response;
        }

        public async Task<Response<AuthenticationResponse>> SocialAuthenticationAsync(SocialAuthenticationRequest request, string ipAddress)
        {
            if (request.Provider.Equals("Google", StringComparison.CurrentCultureIgnoreCase))
            {
                var userInfo = await _googleServices.ValidateIdToken(request.Token);
                if (userInfo == null)
                {
                    var error = new ValidationError("token", "Token not valid.");
                    throw new ValidationException(error);
                }
                var response = await AuthenticationWithoutPassword(providerId: userInfo.ProviderUserId, ipAddress: ipAddress);
                return new Response<AuthenticationResponse>(response, $"Authenticated Google {userInfo.Email}");
            }

            if (request.Provider.Equals("Facebook", StringComparison.CurrentCultureIgnoreCase))
            {
                var userInfo = await _facebookService.ValidationAccessToken(request.Token);
                if (userInfo == null)
                {
                    var error = new ValidationError("token", "Token not valid.");
                    throw new ValidationException(error);
                }
                var response = await AuthenticationWithoutPassword(providerId: userInfo.ProviderUserId, ipAddress: ipAddress);
                return new Response<AuthenticationResponse>(response, $"Authenticated Facebook {userInfo.Email}");
            }

            throw new ValidationException(new ValidationError("provider", "Provider not support."));
        }

        public async Task<Response<RegisterResponse>> SocialRegisterAsync(SocialRegisterRequest request)
        {
            if (request.Provider.Equals("Google", StringComparison.CurrentCultureIgnoreCase))
            {
                var userInfo = await _googleServices.ValidateIdToken(request.Token);
                if (userInfo == null)
                {
                    var error = new ValidationError("token", "Token not valid.");
                    throw new ValidationException(error);
                }
                var user = await RegisterSocialAsync(userInfo, request.Role);
                var response = new RegisterResponse
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName
                };
                return new Response<RegisterResponse>(response, $"Authenticated Google {userInfo.Email}");
            }

            if (request.Provider.Equals("Facebook", StringComparison.CurrentCultureIgnoreCase))
            {
                var userInfo = await _facebookService.ValidationAccessToken(request.Token);
                if (userInfo == null)
                {
                    var error = new ValidationError("token", "Token not valid.");
                    throw new ValidationException(error);
                }
                var user = await RegisterSocialAsync(userInfo, request.Role);
                var response = new RegisterResponse
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName
                };
                return new Response<RegisterResponse>(response, $"Authenticated Facebook {userInfo.Email}");
            }

            throw new ValidationException(new ValidationError("provider", "Provider not support."));
        }

        public async Task<Response<RegisterResponse>> UpdateUsername(string userId, string username)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (user.IsChangeUsername)
                {
                    throw new ValidationException(new ValidationError("user", "User can't change username."));
                }
                var findUsername = await _userManager.FindByNameAsync(username);
                if (findUsername != null)
                {
                    throw new ValidationException(new ValidationError("username", $"'{username}' was duplicate."));
                }
                string oldUsername = user.UserName;
                user.UserName = username;
                user.IsChangeUsername = true;
                await _userManager.UpdateAsync(user);
                await _applicationUserService.UpdateUsername(oldUsername, username);
                var response = new RegisterResponse
                {
                    Email = user.Email,
                    UserName = username
                };
                return new Response<RegisterResponse>(response, $"'{user.Email}' was update username to '{username}'");
            }
            var error = new ValidationError("user", "User not valid.");
            throw new ValidationException(error);

        }

        public async Task<Response<string>> SetPassword(SetPasswordRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
                    await _userManager.UpdateAsync(user);
                    return new Response<string>(data: $"Set password for '{user.UserName}' was successfull.");
                }
                throw new ValidationException(new ValidationError("user", "This user can't set password. Try user reset password api."));
            }
            var error = new ValidationError("user", "User not valid.");
            throw new ValidationException(error);
        }
    }

}
