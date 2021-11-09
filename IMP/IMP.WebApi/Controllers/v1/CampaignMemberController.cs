using IMP.Application.Enums;
using IMP.Application.Features.Campaigns.Commands.ProcessCampaignMember;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.CampaignMember)]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Brand")]
    public class CampaignMemberController : BaseApiController
    {
        /// <summary>
        /// Approval for influencer apply to campaign
        /// </summary>
        /// <param name="id">The Campaign member id</param>
        /// <returns></returns>
        [HttpPost("{id}/approval")]
        public async Task<IActionResult> Approval([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ProcessCampaignMemberCommand { CampaignMemberId = id, Status = CampaignMemberStatus.Approved }));
        }

        /// <summary>
        /// Cancel for influencer apply to campaign
        /// </summary>
        /// <param name="id">The Campaign member id</param>
        /// <returns></returns>
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ProcessCampaignMemberCommand { CampaignMemberId = id, Status = CampaignMemberStatus.Cancelled }));
        }
    }
}
