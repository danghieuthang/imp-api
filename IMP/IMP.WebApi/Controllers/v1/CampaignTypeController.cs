using System.Threading.Tasks;
using IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType;
using IMP.Application.Features.CampaignTypes.Commands.DeleteCampaignTypeById;
using IMP.Application.Features.CampaignTypes.Commands.UpdateCampaignType;
using IMP.Application.Features.CampaignTypes.Queries;
using IMP.Application.Features.CampaignTypes.Queries.GetCampaignTypeById;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.CAMPAIGN_TYPE)]
    public class CampaignTypeController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCampaignTypeCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCampaignTypeQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetCampaignTypeByIdQuery { Id = id }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateCampaignTypeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteCampaignTypeByIdCommand { Id = id }));
        }

    }
}