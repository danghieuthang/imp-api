using IMP.Application.Features.ApplicationUsers.Commands.CreateEmailVerify;
using IMP.Application.Features.ApplicationUsers.Commands.UpdateStatus;
using IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation;
using IMP.Application.Features.ApplicationUsers.Commands.UpdateUserPaymentInfo;
using IMP.Application.Features.ApplicationUsers.Commands.VerifyEmail;
using IMP.Application.Features.ApplicationUsers.Queries.GetAllUser;
using IMP.Application.Features.ApplicationUsers.Queries.GetUserById;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
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
        /// Update user information for authenticated user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<ApplicationUserViewModel>), 200)]
        [HttpPut("me")]
        public async Task<IActionResult> Update([FromBody] UpdateUserInformationCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Update payment info for authenticated user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PaymentInforViewModel>), 200)]
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
        [ProducesResponseType(typeof(Response<ApplicationUserViewModel>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        /// <summary>
        /// Get user information of authenticated user
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<ApplicationUserViewModel>), 200)]
        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            var response = await Mediator.Send(new GetUserByIdQuery { Id = id });
            response.Data.Role = _authenticatedUserService.Role;
            return Ok(response);
        }

        /// <summary>
        /// Create verify email request
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), 200)]
        [HttpPost("me/otps")]
        public async Task<IActionResult> CreateVerifyEmail([FromBody] CreateEmailVerifyCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Verify email linked with profile
        /// </summary>
        /// <param name="command">The verify email command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), 200)]
        [HttpPost("me/verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.InfluencerId = id;
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Search influencer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PagedList<ApplicationUserViewModel>>), 200)]
        [Authorize(Roles = "Administrator")]
        [HttpGet("search-influencer")]
        public async Task<IActionResult> SearchInfluecner([FromQuery] GetAllUserQuery query)
        {
            query.SetIsInfluencer(true);
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Search brand
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PagedList<ApplicationUserViewModel>>), 200)]
        [Authorize(Roles = "Administrator")]
        [HttpGet("search-brand")]
        public async Task<IActionResult> SearchBrand([FromQuery] GetAllUserQuery query)
        {
            query.SetIsInfluencer(false);
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Activate a user
        /// </summary>
        /// <param name="id">The id of user.</param>
        /// <returns></returns>
        [HttpPut("{id}/activate")]
        [ProducesResponseType(typeof(Response<ApplicationUserViewModel>), 200)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Activate([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new UpdateUserStatusCommand { Id = id, Status = Application.Enums.UserStatus.Activated }));
        }

        /// <summary>
        /// Disable a user
        /// </summary>
        /// <param name="id">The id of user.</param>
        /// <returns></returns>
        [HttpPut("{id}/disable")]
        [ProducesResponseType(typeof(Response<ApplicationUserViewModel>), 200)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new UpdateUserStatusCommand { Id = id, Status = Application.Enums.UserStatus.Disabled }));
        }

    }
}
