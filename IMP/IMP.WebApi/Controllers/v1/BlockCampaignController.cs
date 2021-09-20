using System.Threading.Tasks;
using IMP.Application.Features.BlockCampaigns.Commands.CreateBlockCampaign;
using IMP.Application.Features.BlockCampaigns.Commands.DeleteBlockCampaign;
using IMP.Application.Features.BlockCampaigns.Commands.UpdateBlockCampaign;
using IMP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.BlockCampaign)]
    [Authorize]
    public class BlockCampaignController : BaseApiController
    {
        private readonly int _appId;
        public BlockCampaignController(IAuthenticatedUserService authenticatedUserService)
        {
            _appId = 0;
            int.TryParse(authenticatedUserService.AppId, out _appId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlockCampaignCommand command)
        {
            command.InfluencerId = _appId;
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Create([FromRoute] int id, [FromBody] UpdateBlockCampaignCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.InfluencerId = _appId;
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var command = new DeleteBlockCampaignCommand
            {
                Id = id,
                InfluencerId = _appId
            };
            return Ok(await Mediator.Send(command));
        }
    }
}