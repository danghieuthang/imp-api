using System.Threading.Tasks;
using IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.CAMPAIGN_TYPE)]
    public class CampaignTypeController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateCampaignTypeCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}