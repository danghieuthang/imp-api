﻿using IMP.Application.Models;
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
using IMP.Infrastructure.Identity.Reponsitories;
using IMP.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Cache;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IGoogleService _googleServices;
        private readonly IFacebookService _facebookService;
        public AccountService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<User> signInManager,
            IEmailService emailService,
            IRefreshTokenRepository refreshTokenRepository,
            IApplicationUserService applicationUserService,
            IGoogleService googleServices,
            IFacebookService facebookService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            this._emailService = emailService;
            this._refreshTokenRepository = refreshTokenRepository;
            this._applicationUserService = applicationUserService;
            this._googleServices = googleServices;
            this._facebookService = facebookService;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
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
            _ = _refreshTokenRepository.AddAsync(refreshTokenDomain);

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
        private async Task<User> RegisterSocialAsync(ProviderUserDetail providerUser)
        {
            string username = await GenerateUnDuplicateUserName(providerUser.Name);

            var user = new User
            {
                Email = providerUser.Email,
                FirstName = providerUser.FirstName,
                LastName = providerUser.LastName,
                UserName = username,
                EmailConfirmed = true
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(providerUser.Email);
            if (userWithSameEmail == null)
            {
                // Create Application User
                var applicationUser = await _applicationUserService.CreateUser(user.UserName);
                // Add Application User ref to Identity User
                if (applicationUser != null)
                {
                    user.ApplicationUserId = applicationUser.Id;
                }

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Fan.ToString());
                    return user;
                }
                else
                {
                    // Delete Application User if create fail
                    await _applicationUserService.DeleteUser(user.UserName);
                    var errors = result.Errors.Select(x =>
                    new ValidationError(x.Code, x.Description)).ToList();
                    throw new ValidationException(errors);
                }
            }
            else
            {
                throw new ApiException($"Email {providerUser.Email } is already registered.");
            }
        }
        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                // Create Application User
                var applicationUser = await _applicationUserService.CreateUser(user.UserName);
                // Add Application User ref to Identity User
                if (applicationUser != null)
                {
                    user.ApplicationUserId = applicationUser.Id;
                }

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Influencer.ToString());
                    var verificationUri = await SendVerificationEmail(user, origin);
                    //TODO: Attach Email Service here and configure it via appsettings
                    await _emailService.SendAsync(new Application.Models.Email.EmailRequest() { To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                    return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                {
                    // Delete Application User if create fail
                    await _applicationUserService.DeleteUser(user.UserName);

                    var errors = result.Errors.Select(x =>
                    new ValidationError(x.Code, x.Description)).ToList();
                    throw new ValidationException(errors);
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }
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
            var route = "http://localhost:57712/api/accounts/confirm-email/";
            var _enpointUri = new Uri(string.Concat(route, $"{origin}"));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
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
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
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
            var entity = await _refreshTokenRepository.GetRefreshToken(refreshToken, ipAddress);
            if (entity == null) throw new ApiException($"Refresh token no.");

            if (!entity.IsActive)
            {
                await _refreshTokenRepository.DeleteAsync(entity);
                var error = new ValidationError("refresh_token", "Token was expired.");
                throw new ValidationException(error);
            }

            var user = entity.User;

            var newJwtToken = await GenerateJWToken(entity.User);
            var newRefreshToken = GenerateRefreshToken(ipAddress);


            #region update refresh token after refresh
            entity.Revoked = DateTime.UtcNow;
            entity.RevokedByIp = ipAddress;
            entity.ReplacedByToken = newRefreshToken.Token;
            await _refreshTokenRepository.UpdateAsync(entity);
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

        public async Task<Response<string>> RevokeToken(string refreshToken, string ipaAddress)
        {
            var entity = await _refreshTokenRepository.GetRefreshToken(refreshToken, ipaAddress);
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
            await _refreshTokenRepository.UpdateAsync(entity);
            return new Response<string>(entity.User.Email, $"Refresh token was revoked.");
        }
        private async Task<AuthenticationResponse> AuthenticationWithoutPassword(string ipAddress, string username = null, string email = null)
        {
            User user;
            if (username != null)
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
            _ = _refreshTokenRepository.AddAsync(refreshTokenDomain);
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
                var response = await AuthenticationWithoutPassword(email: userInfo.Email, ipAddress: ipAddress);
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
                var response = await AuthenticationWithoutPassword(email: userInfo.Email, ipAddress: ipAddress);
                return new Response<AuthenticationResponse>(response, $"Authenticated Facebook {userInfo.Email}");
            }

            throw new ValidationException(new ValidationError("provider", "Provider not support."));
        }

        public async Task<Response<RegisterResponse>> SocialRegisterAsync(SocialAuthenticationRequest request)
        {
            if (request.Provider.Equals("Google", StringComparison.CurrentCultureIgnoreCase))
            {
                var userInfo = await _googleServices.ValidateIdToken(request.Token);
                if (userInfo == null)
                {
                    var error = new ValidationError("token", "Token not valid.");
                    throw new ValidationException(error);
                }
                var user = await RegisterSocialAsync(userInfo);
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
                var user = await RegisterSocialAsync(userInfo);
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
    }

}
