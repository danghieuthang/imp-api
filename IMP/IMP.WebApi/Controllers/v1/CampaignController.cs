using IMP.Application.Features.CampaignMembers.Queries.GetCampaignMemberOfCampaignByInfluencerId;
using IMP.Application.Features.Campaigns.Commands.ApplyToCampaign;
using IMP.Application.Features.Campaigns.Commands.ApprovalCampaign;
using IMP.Application.Features.Campaigns.Commands.CancelCampaign;
using IMP.Application.Features.Campaigns.Commands.CompletedCreateCampaign;
using IMP.Application.Features.Campaigns.Commands.CreateCampaign;
using IMP.Application.Features.Campaigns.Commands.CreateDraftCampaign;
using IMP.Application.Features.Campaigns.Commands.DeleteCampaign;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaign;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignActivities;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignInfluencerConfiguration;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignTargetConfiguration;
using IMP.Application.Features.Campaigns.Queries.GetAllCampaignMemberByCampaignId;
using IMP.Application.Features.Campaigns.Queries.GetAllCampaigns;
using IMP.Application.Features.Campaigns.Queries.GetCampaignById;
using IMP.Application.Features.Campaigns.Queries.GetCampaignByInfluencerId;
using IMP.Application.Features.Campaigns.Queries.GetSumaryMemberByCampaignId;
using IMP.Application.Features.VoucherCodes.Queries.GetAllVoucherCodeOfCampaignMember;
using IMP.Application.Features.Vouchers.Queries;
using IMP.Application.Features.Vouchers.Queries.GetAllVoucherAssignedByCampaign;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Campaign)]
    public class CampaignController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ILogger _logger = Log.ForContext<CampaignController>();

        public CampaignController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        /// <summary>
        /// Query list campaign
        /// </summary>
        /// <param name="query">The Get All Campaign Query</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<CampaignViewModel>>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetCampaigns([FromQuery] GetAllCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Query list campaign of influencer logged
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<CampaignViewModel>>), 200)]
        [HttpGet("me")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetCampaignOfInfluencer([FromQuery] GetCampaignByInfluencerIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get campaign by bio link
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<CampaignViewModel>>), 200)]
        [HttpGet("search-campaign-by-biolink")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCampaignOfInfluencer([FromQuery] GetCampaignByBioLinkQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get a campaign by id
        /// </summary>
        /// <param name="id">The campaign id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetCampaignByIdQuery { Id = id }));
        }

        /// <summary>
        /// Get a campaign to notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignNotificationViewModel>), 200)]
        [HttpGet("{id}/notification-info")]
        public async Task<IActionResult> GetCampaignNotificationById([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetCampaignNotificationByIdQuery { Id = id }));
        }

        /// <summary>
        /// Create a campaign
        /// </summary>
        /// <param name="command">The Create Campaign Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 201)]
        [HttpPost]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Create()
        {
            return StatusCode(201, await Mediator.Send(new CreateDraftCampaignCommand()));
        }

        /// <summary>
        /// Update campaign target configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [HttpPut("{id}/target-configuration")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> UpdateTargetConfiguration([FromRoute] int id, [FromBody] UpdateTargetConfigurationCommand command)
        {
            if (id! != command.CampaignId)
            {
                return BadRequest();
            }
            return StatusCode(200, await Mediator.Send(command));
        }

        /// <summary>
        /// Update campaign influencer configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [HttpPut("{id}/influencer-configuration")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> UpdateInfluencerConfiguration([FromRoute] int id, [FromBody] UpdateInfluencerConfigurationCommand command)
        {
            if (id != command.CampaignId)
            {
                return BadRequest();
            }
            return StatusCode(200, await Mediator.Send(command));
        }

        /// <summary>
        /// Update campaign information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>

        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [HttpPut("{id}")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> UpdateInformation([FromRoute] int id, [FromBody] UpdateCampaignInformationCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Update campaign activity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}/activities")]
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> UpdateActivities([FromRoute] int id, [FromBody] UpdateCampaignActivitiesCommand command)
        {
            if (id != command.CampaignId)
            {
                return BadRequest();
            }
            var request = command.Clone();
            var response = await Mediator.Send(command);

            _ = Task.Run(() =>
            {
                string reqs = JsonConvert.SerializeObject(request);
                string res = JsonConvert.SerializeObject(response.Data);
                string logContent = $"User: {_authenticatedUserService.ApplicationUserId}:\n \t\tRequest: {reqs}\n\t\tResponse: {res}";
                _logger.Information(logContent);
            });

            return Ok(response);
        }

        /// <summary>
        /// Complete create a campaign(change status Draft => Pending)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/complete-created")]
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> CompleteCreatet([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new CompletedCreateCampaignCommand { Id = id }));
        }

        /// <summary>
        /// Approval campaign(change status Pending => Approved)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/approval")]
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ApprovalCampaign([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ApprovalCampaignCommand { Id = id }));
        }

        /// <summary>
        /// Cancel campaign(change status Pending => Canceled)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CancelCampaign([FromRoute] int id, [FromBody] CancelCampaignCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Change status cancel/approved to pending
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}/waiting")]
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> WaitingCampaign([FromRoute] int id, [FromBody] WaitingCampaignCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Get all voucher of a campaign
        /// </summary>
        /// <param name="id">The id of campaign</param>
        /// <param name="isGetVoucherCodes">If true then include voucher code</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<VoucherViewModel>>), 200)]
        [HttpGet("{id}/vouchers")]
        public async Task<IActionResult> GetVoucherOfCampaign([FromRoute] int id, [FromQuery(Name = "is_get_voucher_codes")] bool isGetVoucherCodes)
        {
            return Ok(await Mediator.Send(new GetAllVoucherByCampaignIdQuery { CampaignId = id, IsGetVoucherCodes = isGetVoucherCodes }));
        }

        /// <summary>
        /// Get all voucher that have voucher code was assigned for campaign member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<VoucherViewModel>>), 200)]
        [HttpGet("{id}/vouchers-assigned-for-member")]
        public async Task<IActionResult> GetVoucherAssignedForMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllVoucherAssignedByCampaignQuery { CampaignId = id }));
        }


        /// <summary>
        /// Influencer apply to campaign
        /// </summary>
        /// <param name="id">The id of campaign</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPost("{id}/apply")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> ApplyToCampaign([FromRoute] int id)
        {
            var response = await Mediator.Send(new ApplyToCampaignCommand { CampaignId = id });
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Change pending campaign to draft campaign
        /// </summary>
        /// <param name="id">The id of campaign</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignViewModel>), 200)]
        [HttpPut("{id}/draft")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> DraftCampaign([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DraftCampagnPendingCommand
            {
                Id = id
            }));
        }

        /// <summary>
        /// Search influencer in campaign
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PagedList<CampaignMemberViewModel>>), 200)]
        [HttpGet("{id}/members")]
        //[Authorize(Roles = "Brand")]
        public async Task<IActionResult> SearchMember([FromQuery] GetAllCampaignMemberByCampaignIdQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        /// <summary>
        /// Get campaign member in campaign of authenticated user
        /// </summary>
        /// <param name="id">The id of campaign</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignMemberViewModel>), 200)]
        [HttpGet("{id}/members/me")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> SearchMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetCampaignMemberOfCampaignByInfluencerIdQuery { CampaignId = id }));
        }

        /// <summary>
        /// Get all voucher code in a campaign of authenticated user
        /// </summary>
        /// <param name="id">The campaign id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<VoucherCodeViewModel>>), 200)]
        [HttpGet("{id}/members/me/voucher-codes")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetVoucherCodeOfMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllVoucherCodeOfCampaignMemberQuery { CampaignId = id }));
        }


        /// <summary>
        /// Delete campaign
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteCampaignCommand { Id = id }));
        }

        /// <summary>
        /// Check the logged in user is suitable for the campaign
        /// </summary>
        /// <param name="id">The id of campaign</param>
        /// <param name="checkSuitable">Is check suitable influencer</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpGet("{id}/check-suitable")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> CheckIsSuitableWithCampaign([FromRoute] int id, [FromQuery(Name = "check_suitable")] bool checkSuitable = true)
        {
            return Ok(await Mediator.Send(new CheckCampatibleWithCampaignCommand { CampaignId = id, IsCheckSuitable = checkSuitable }));
        }

        /// <summary>
        /// Get summary data about campaign members of a campaign
        /// </summary>
        /// <param name="id">The campaign id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<SummaryCampaignMemberViewModel>), 200)]
        [HttpGet("{id}/summary-members")]
        [AllowAnonymous]
        public async Task<IActionResult> SumaryMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetSumaryMemberByCampaignIdQuery { CampaignId = id }));
        }
    }
}
