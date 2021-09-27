using IMP.Application.Features.BlockTypes.Commands.CreateBlockType;
using IMP.Application.Features.BlockTypes.Commands.DeleteBlockType;
using IMP.Application.Features.BlockTypes.Commands.UpdateBlockType;
using IMP.Application.Features.BlockTypes.Queries.GetAllBlockType;
using IMP.Application.Features.BlockTypes.Queries.GetBlockTypeById;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.BlockType)]
    public class BlockTypeController : BaseApiController
    {
        /// <summary>
        /// Get all block type
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<BlockTypeViewModel>>), 200)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllBlockTypeQuery()));
        }

        /// <summary>
        /// Get a block type by id
        /// </summary>
        /// <param name="id">The id of block type</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockTypeViewModel>), 200)]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetBlockTypeByIdQuery { Id = id }));
        }

        /// <summary>
        /// Craete new a block type
        /// </summary>
        /// <param name="command">The Create Block Type Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockTypeViewModel>), 201)]
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateBlockTypeCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Update a block type
        /// </summary>
        /// <param name="id">The block type id</param>
        /// <param name="command">The Update Block Type Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<BlockTypeViewModel>), 200)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromForm] UpdateBlockTypeCommand command)
        {
            if (command.Id != id)
            {
                return BadRequest();
            }
            return StatusCode(200, await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a block type by id
        /// </summary>
        /// <param name="id">The block type id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteBlockTypeCommand { Id = id }));
        }

    }
}
