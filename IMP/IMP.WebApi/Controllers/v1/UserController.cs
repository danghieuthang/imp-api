using IMP.Application.Features.ApplicationUsers.Commands.CreateEmailVerify;
using IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation;
using IMP.Application.Features.ApplicationUsers.Commands.UpdateUserPaymentInfo;
using IMP.Application.Features.ApplicationUsers.Commands.VerifyEmail;
using IMP.Application.Features.ApplicationUsers.Queries.GetUserById;
using IMP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.User)]
    [Authorize(Roles = "Influencer, Brand, Administrator")]
    public class UserController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UserController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("me")]
        public async Task<IActionResult> Update([FromBody] UpdateUserInformationCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.Id = id;

            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Update payment info for autheticated user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("me/payment-info")]
        public async Task<IActionResult> Update([FromBody] UpdateUserPaymentInfoCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.ApplicationUserId = id;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Get user information by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        /// <summary>
        /// Get user information of authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        /// <summary>
        /// Create verify email request
        /// </summary>
        /// <returns></returns>
        [HttpPost("me/otps")]
        public async Task<IActionResult> CreateVerifyEmail()
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            return StatusCode(201, await Mediator.Send(new CreateEmailVerifyCommand { InfluencerId = id }));
        }

        /// <summary>
        /// Verify email
        /// </summary>
        /// <param name="command">The verify email command</param>
        /// <returns></returns>
        [HttpPost("me/verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.InfluencerId = id;
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
