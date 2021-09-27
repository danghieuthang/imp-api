using IMP.Application.Enums;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
using IMP.Application.Features.Platforms.Commands.DeletePlatformById;
using IMP.Application.Features.Platforms.Commands.UpdatePlatform;
using IMP.Application.Features.Platforms.Queries;
using IMP.Application.Features.Platforms.Queries.GetAllPlatforms;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
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
    [Route(RouterConstants.Platform)]
    public class PlatformController : BaseApiController
    {
        /// <summary>
        /// Get list platform
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PlatformViewModel>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllPlatformsQuery()));
        }
        /// <summary>
        /// Get platform by id
        /// </summary>
        /// <param name="id">The id of platform</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PlatformViewModel>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetPlatformByIdQuery { Id = id }));
        }

        /// <summary>
        /// Create new platform
        /// </summary>
        /// <param name="command">The Create Platform Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PlatformViewModel>), 201)]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromForm] CreatePlatformCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Update platform
        /// </summary>
        /// <param name="id">The id of platform</param>
        /// <param name="command">The Update Platform Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PlatformViewModel>), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdatePlatformCommand command)
        {
            if (command.Id != id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete platform by id
        /// </summary>
        /// <param name="id">The platform id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeletePlatformByIdCommand { Id = id }));
        }
    }
}
