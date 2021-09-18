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

namespace IMP.WebApi.Controllers
{
    [Route(RouterConstants.ACCOUNT)]
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

        [HttpPost("social-authenticate")]
        public async Task<IActionResult> AuthenticateWithSocial(SocialAuthenticationRequest request)
        {
            return Ok(await _accountService.SocialAuthenticationAsync(request, GenerateIPAddress()));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return StatusCode(201, await _accountService.RegisterAsync(request, origin));
        }

        [HttpPost("social-register")]
        public async Task<IActionResult> SocialRegisterAsync(SocialRegisterRequest request)
        {
            return StatusCode(201, await _accountService.SocialRegisterAsync(request));
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            return Ok(await _accountService.ResetPassword(model));
        }

        /// <summary>
        /// This api only for user register with social
        /// Each social account has only one turn change username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("change-username")]
        [Authorize(Roles = "Influencer, Brand")]
        public async Task<IActionResult> ChangeUsername([FromBody] string username)
        {
            return Ok(await _accountService.UpdateUsername(_authenticatedUserService.UserId, username));
        }

        [HttpPost("set-password")]
        [Authorize(Roles = "Influencer, Brand")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest request)
        {
            return Ok(await _accountService.SetPassword(request, _authenticatedUserService.UserId));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = Request.Cookies["refresh-token"];
            }
            return Ok(await _accountService.RefreshToken(refreshToken, GenerateIPAddress()));
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken()
        {
            var refeshToken = Request.Cookies["refresh-token"];
            return Ok(await _accountService.RevokeToken(refeshToken, GenerateIPAddress()));
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