using IMP.Application.Models.Account;
using IMP.Application.Wrappers;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<AuthenticationResponse>> RefreshToken(string refreshToken, string ipaAddress);
        Task<Response<string>> RevokeToken(string refreshToken, string ipaAddress);

        Task<Response<AuthenticationResponse>> SocialAuthenticationAsync(SocialAuthenticationRequest request, string ipAddress);
        Task<Response<RegisterResponse>> SocialRegisterAsync(SocialAuthenticationRequest request);
        Task<Response<RegisterResponse>> UpdateUsername(string userId, string username);
        Task<Response<string>> SetPassword(SetPasswordRequest request, string userId);
    }
}
