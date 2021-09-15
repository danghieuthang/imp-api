using IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation;
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
    [Route(RouterConstants.USER)]
    [Authorize(Roles = "Influencer, Brand, Administrator")]
    public class UserController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public UserController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpPut("me")]
        public async Task<IActionResult> Update([FromBody] UpdateUserInformationCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.Id = id;

            return Ok(await Mediator.Send(command));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

    }
}
