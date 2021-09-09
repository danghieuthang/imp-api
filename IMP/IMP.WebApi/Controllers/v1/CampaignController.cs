using IMP.Application.Features.Campaigns.Commands.CreateCampaign;
using IMP.Application.Features.Campaigns.Queries.GetAllCampaigns;
using IMP.Application.Interfaces;
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
    [Route(RouterConstants.CAMPAIGN)]
    public class CampaignController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public CampaignController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaigns([FromQuery] GetAllCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Create([FromBody] CreateCampaignCommand command)
        {
            int brandId = 0;
            _ = int.TryParse(_authenticatedUserService.AppId, out brandId);
            command.BrandId = brandId;
            return StatusCode(201, await Mediator.Send(command));
        }

    }
}
