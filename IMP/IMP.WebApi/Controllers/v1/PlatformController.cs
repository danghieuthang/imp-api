using IMP.Application.Enums;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
using IMP.Application.Features.Platforms.Commands.DeletePlatformById;
using IMP.Application.Features.Platforms.Commands.UpdatePlatform;
using IMP.Application.Features.Platforms.Queries;
using IMP.Application.Features.Platforms.Queries.GetAllPlatforms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.PLATFORM)]
    public class PlatformController : BaseApiController
    {
        /// <summary>
        /// Get list platform
        /// </summary>
        /// <param name="query">The Get Platform Parameter</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllPlatformsQuery query)
        {
            var user = Request.HttpContext.User;
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetPlatformByIdQuery { Id = id }));
        }

        /// <summary>
        /// Create new platform
        /// </summary>
        /// <param name="command">The CreatePlatformCommand</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromForm] CreatePlatformCommand command)
        {
            return StatusCode(2021, await Mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePlatformCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeletePlatformByIdCommand { Id = id }));
        }
    }
}
