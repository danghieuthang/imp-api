using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType;
using IMP.Application.Features.CampaignTypes.Commands.DeleteCampaignTypeById;
using IMP.Application.Features.CampaignTypes.Commands.UpdateCampaignType;
using IMP.Application.Features.CampaignTypes.Queries;
using IMP.Application.Features.CampaignTypes.Queries.GetCampaignTypeById;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.CampaignType)]
    public class CampaignTypeController : BaseApiController
    {
        /// <summary>
        /// Create a campaign type
        /// </summary>
        /// <param name="command">The Create Campaign Type Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignTypeViewModel>), 201)]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromForm] CreateCampaignTypeCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Get all campaign type
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<CampaignTypeViewModel>>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllCampaignTypeQuery()));
        }

        /// <summary>
        /// Get a campaign type by id
        /// </summary>
        /// <param name="id">The id of campaign type</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignTypeViewModel>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetCampaignTypeByIdQuery { Id = id }));
        }

        /// <summary>
        /// Update a campaign type
        /// </summary>
        /// <param name="id">The id of campaign type</param>
        /// <param name="command">The Update Campaign Type Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignTypeViewModel>), 200)]
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateCampaignTypeCommand command)
        {
            if (command.Id != id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a campaign type by id
        /// </summary>
        /// <param name="id">The id of campaign type</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignTypeViewModel>), 200)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteCampaignTypeByIdCommand { Id = id }));
        }

    }
}