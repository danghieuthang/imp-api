using IMP.Application.Features.BlockTypes.Commands.CreateBlockType;
using IMP.Application.Features.BlockTypes.Commands.DeleteBlockType;
using IMP.Application.Features.BlockTypes.Commands.UpdateBlockType;
using IMP.Application.Features.BlockTypes.Queries.GetAllBlockType;
using IMP.Application.Features.BlockTypes.Queries.GetBlockTypeById;
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
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetAllBlockTypeQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetBlockTypeByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateBlockTypeCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromForm] UpdateBlockTypeCommand command)
        {
            if (command.Id != id)
            {
                return BadRequest();
            }
            return StatusCode(201, await Mediator.Send(command));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteBlockTypeCommand { Id = id }));
        }

    }
}
