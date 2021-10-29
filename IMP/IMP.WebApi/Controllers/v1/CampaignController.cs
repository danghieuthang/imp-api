using IMP.Application.Features.Campaigns.Commands.CreateCampaign;
using IMP.Application.Features.Campaigns.Commands.CreateDraftCampaign;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignInfluencerConfiguration;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignTargetConfiguration;
using IMP.Application.Features.Campaigns.Queries.GetAllCampaigns;
using IMP.Application.Features.Campaigns.Queries.GetCampaignById;
using IMP.Application.Features.Vouchers.Queries;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        /// Get all voucher of a campaign
        /// </summary>
        /// <param name="id">The id of campaign</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<VoucherViewModel>>), 200)]
        [HttpGet("{id}/vouchers")]
        public async Task<IActionResult> GetVoucherOfCampaign([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllVoucherByCampaignIdQuery { CampaignId = id }));
        }

    }
}
