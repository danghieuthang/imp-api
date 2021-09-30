using IMP.Application.Models.Account;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IMP.Application.Wrappers;
using System.Net;

namespace IMP.WebApi.Controllers
{
    [Route(RouterConstants.Account)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public AccountController(IAccountService accountService, IAuthenticatedUserService authenticatedUserService)
        {
            _accountService = accountService;
            _authenticatedUserService = authenticatedUserService;
        }
        /// <summary>
        /// Authenticate with email and password
        /// </summary>
        /// <param name="request">The Authentication Request</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<AuthenticationResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = await _accountService.AuthenticateAsync(request, GenerateIPAddress());
            if (response.Succeeded)
            {
                SetRefreshToken(response.Data.RefreshToken);
            }
            return Ok(response);
        }

        /// <summary>
        /// Authenticate with social platform
        /// </summary>
        /// <param name="request">The Social Authentication Request</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<AuthenticationResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("social-authenticate")]
        public async Task<IActionResult> AuthenticateWithSocial(SocialAuthenticationRequest request)
        {
            return Ok(await _accountService.SocialAuthenticationAsync(request, GenerateIPAddress()));
        }

        /// <summary>
        /// Register account with email and password
        /// </summary>
        /// <param name="request">The Register Request </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.Created)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return StatusCode(201, await _accountService.RegisterAsync(request, origin));
        }

        /// <summary>
        /// Register account with social platform
        /// </summary>
        /// <param name="request">The Social Register Request </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.Created)]
        [HttpPost("social-register")]
        public async Task<IActionResult> SocialRegisterAsync(SocialRegisterRequest request)
        {
            return StatusCode(201, await _accountService.SocialRegisterAsync(request));
        }

        /// <summary>
        /// Confirm email when register account with email(This api call when click link confirm in email)
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="code">The token </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }

        /// <summary>
        /// Request fotgot password
        /// </summary>
        /// <param name="model">The Forgot Password Request</param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }
        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="model">The Reset Password Request</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _accountService.ResetPassword(model));
        }

        /// <summary>
        /// This api only for user register with social.
        /// Each social account has only one turn change username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<RegisterResponse>), (int)HttpStatusCode.OK)]
        [HttpPut("change-username")]
        [Authorize(Roles = "Influencer, Brand")]
        public async Task<IActionResult> ChangeUsername([FromBody] string username)
        {
            return Ok(await _accountService.UpdateUsername(_authenticatedUserService.UserId, username));
        }

        /// <summary>
        /// Set password for social account(Call only once)
        /// </summary>
        /// <param name="request">The Set Password Request</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
        [HttpPost("set-password")]
        [Authorize(Roles = "Influencer, Brand")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
        {
            return Ok(await _accountService.SetPassword(request, _authenticatedUserService.UserId));
        }

        /// <summary>
        /// Create new access token from refresh token. User can set refresh token in here or cookie
        /// </summary>
        /// <param name="request">The Refresh Token Request</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<AuthenticationResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                request.RefreshToken = Request.Cookies["refresh-token"];
            }
            return Ok(await _accountService.RefreshToken(request.RefreshToken, GenerateIPAddress()));
        }


        /// <summary>
        /// Revoke token
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken()
        {
            var refeshToken = Request.Cookies["refresh-token"];
            return Ok(await _accountService.RevokeToken(refeshToken, GenerateIPAddress()));
        }

        /// <summary>
        /// Check jwt is expired
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [HttpGet("check-expired")]
        [Authorize]
        public IActionResult CheckExpire()
        {
            return Ok(200);
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void SetRefreshToken(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("refresh-token", refreshToken, cookieOptions);
        }
    }
}