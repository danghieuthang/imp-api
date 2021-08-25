using IMP.Application.Enums;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
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

        /// <summary>
        /// Create new platform
        /// </summary>
        /// <param name="command">The CreatePlatformCommand</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] CreatePlatformCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
