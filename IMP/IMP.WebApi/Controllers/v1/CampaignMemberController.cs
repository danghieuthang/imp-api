using IMP.Application.Enums;
using IMP.Application.Features.CampaignMembers.Commands.AcceptInviteJoinCampaign;
using IMP.Application.Features.CampaignMembers.Commands.SendRewardToInfluencer;
using IMP.Application.Features.CampaignMembers.Queries.GetCampaignMemberById;
using IMP.Application.Features.CampaignMembers.Queries.Reports;
using IMP.Application.Features.Campaigns.Commands.ProcessCampaignMember;
using IMP.Application.Features.MemberActivities.Queries.GetAllMemberActivitesOfUser;
using IMP.Application.Features.Vouchers.Queries.GetAllVoucherOfCampaignMember;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.CampaignMember)]
    [ApiVersion("1.0")]
    [Authorize()]
    public class CampaignMemberController : BaseApiController
    {
        /// <summary>
        /// Approval for influencer apply to campaign
        /// </summary>
        /// <param name="id">The Campaign member id</param>
        /// <returns></returns>
        [HttpPost("{id}/approval")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Approval([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ProcessCampaignMemberCommand { CampaignMemberId = id, Status = CampaignMemberStatus.Approved }));
        }

        /// <summary>
        /// Cancel for influencer apply to campaign
        /// </summary>
        /// <param name="id">The Campaign member id</param>
        /// <param name="request">The Cancel Request</param>
        /// <returns></returns>
        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Cancel([FromRoute] int id, [FromBody] CampaignMemberCancelRequest request)
        {
            return Ok(await Mediator.Send(new ProcessCampaignMemberCommand { CampaignMemberId = id, Status = CampaignMemberStatus.Cancelled, Note = request.Note }));
        }

        /// <summary>
        /// Influencer accept invited join to campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/accept-invited")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> AcceptInvited([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new AcceptInviteJoinCampaignCommand { Id = id }));
        }

        /// <summary>
        /// Reject invited join to campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/reject-invited")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> RejectInvite([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new RejectInviteJoinCampaignCommand { Id = id }));
        }

        /// <summary>
        /// Get all member activies of campaign member
        /// </summary>
        /// <param name="id">The campaign member id</param>
        /// <returns></returns>
        [HttpGet("{id}/member-activities")]
        [ProducesResponseType(typeof(Response<List<MemberActivityViewModel>>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> GetMemberActivityOfCampaignMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllMemberAcitivitiesOfUserQuery { CampaignMemberId = id }));
        }

        /// <summary>
        /// Get campaign member by id
        /// </summary>
        /// <param name="id">The id of campaign member</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<CampaignMemberViewModel>), 200)]
        [Authorize()]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetCampaignMemberByIdQuery { Id = id }));
        }

        /// <summary>
        /// Get all voucher of campaign member
        /// </summary>
        /// <param name="id">The campaign member id</param>
        /// <returns></returns>
        [HttpGet("{id}/vouchers")]
        [ProducesResponseType(typeof(Response<List<VoucherViewModel>>), 200)]
        [Authorize(Roles = "Brand,Influencer")]
        public async Task<IActionResult> GetVoucherMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllVoucherOfCampaignMemberQuery { CampaignMemberId = id }));
        }

        /// <summary>
        /// Send reward for member who completed all activity
        /// </summary>
        /// <param name="id">The campaign member id</param>
        /// <returns></returns>
        [HttpPost("{id}/send-reward")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> SendRewardToMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new SendRewardToInfluencerCommand { CampaignMemberId = id }));
        }

        /// <summary>
        /// Get report of a campaign member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/report")]
        [ProducesResponseType(typeof(Response<CampaignMemberReportViewModel>), 200)]
        [Authorize(Roles = "Brand,Influencer")]
        public async Task<IActionResult> Report([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetReportOfCampaignMemberQuery { Id = id }));
        }


    }
}
