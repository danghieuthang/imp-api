using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMP.Application.Features.BlockCampaigns.Queries.GetAllBlockCampaignByBlockId;
using IMP.Application.Features.BlockPlatforms.Queries.GetAllBlockPlatformByBlockId;
using IMP.Application.Features.Blocks.Commands.CreateBlock;
using IMP.Application.Features.Blocks.Commands.DeleteBlock;
using IMP.Application.Features.Blocks.Commands.UpdateBlock;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Block)]
    public class BlockController : BaseApiController
    {
        private readonly int _appId;
        public BlockController(IAuthenticatedUserService authenticatedUserService)
        {
            _appId = 0;
            int.TryParse(authenticatedUserService.AppId, out _appId);
        }
        /// <summary>
        /// Get all block platform of a block
        /// </summary>
        /// <param name="id">The block id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<BlockPlatformViewModel>>), 200)]
        [HttpGet("{id}/block-platforms")]
        public async Task<IActionResult> GetBlockPlatform([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllBlockPlatformByBlockIdQuery { BlockId = id }));
        }

        /// <summary>
        /// Get all block campaign of block
        /// </summary>
        /// <param name="id">The id of block campaign</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<BlockCampaignViewModel>>), 200)]
        [HttpGet("{id}/block-campaigns")]
        public async Task<IActionResult> GetBlockCampaign([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllBlockCampaignByBlockIdQuery { BlockId = id }));
        }

        /// <summary>
        /// Create a new block
        /// </summary>
        /// <param name="command">The Create Block Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockViewModel>), 201)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlockCommand command)
        {
            command.InfluencerId = _appId;
            return StatusCode(201, await Mediator.Send(command));
        }
        /// <summary>
        /// Update a block
        /// </summary>
        /// <param name="id">The block id</param>
        /// <param name="command">The Update Block Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockViewModel>), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBlockCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.InfluencerId = _appId;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a block
        /// </summary>
        /// <param name="id">The block id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteBlockCommand
            {
                Id = id,
                InfluencerId = _appId
            };
            return Ok(await Mediator.Send(command));
        }
    }
}