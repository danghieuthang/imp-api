using System.Threading.Tasks;
using IMP.Application.Features.BlockPlatforms.Commands.CreateBlockPlatform;
using IMP.Application.Features.BlockPlatforms.Commands.DeleteBlockPlatform;
using IMP.Application.Features.BlockPlatforms.Commands.UpdateBlockPlatform;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.BlockPlatform)]
    [Authorize]
    public class BlockPlatformController : BaseApiController
    {
        private readonly int _appId;
        public BlockPlatformController(IAuthenticatedUserService authenticateUserService)
        {
            _appId = 0;
            int.TryParse(authenticateUserService.AppId, out _appId);
        }

        /// <summary>
        /// Create a new block platoform
        /// </summary>
        /// <param name="command">The Create Block Platform Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockPlatformViewModel>), 201)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlockPlatformCommand command)
        {
            command.InfluencerId = _appId;
            return StatusCode(201, await Mediator.Send(command));
        }


        /// <summary>
        /// Update a block platform
        /// </summary>
        /// <param name="id">The id of block platform</param>
        /// <param name="command">The Update Block Platform Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockPlatformViewModel>), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Create([FromRoute] int id, [FromBody] UpdateBlockPlatformCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.InfluencerId = _appId;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a block platform by id
        /// </summary>
        /// <param name="id">The id of block platform</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var command = new DeleteBlockPlatformCommand
            {
                Id = id,
                InfluencerId = _appId
            };
            return Ok(await Mediator.Send(command));
        }

    }
}