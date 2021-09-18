using IMP.Application.Features.BlockTypes.Commands.DeleteBlockType;
using IMP.Application.Features.InfluencerPlatforms.Commands.CreateInfluencerPlatform;
using IMP.Application.Features.InfluencerPlatforms.Commands.DeleteInfluencerPlatformById;
using IMP.Application.Features.InfluencerPlatforms.Commands.RequestVerifyInfluencerPlatform;
using IMP.Application.Features.InfluencerPlatforms.Commands.UpdateInlfuencerPlatform;
using IMP.Application.Features.InfluencerPlatforms.Queries.GetAllInfluencerPlatformByInfluencerId;
using IMP.Application.Interfaces;
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
    [Route(RouterConstants.INFLUENCER_PLATFORM)]
    public class InfluencerPlatformController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        int influencerId = 0;

        public InfluencerPlatformController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
            _ = int.TryParse(_authenticatedUserService.AppId, out influencerId);
        }

        [HttpGet]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllInfluencerPlatformQuery { InfluencerId = influencerId }));
        }

        [HttpPost]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Create([FromBody] CreateInfluencerPlatformCommand command)
        {
            command.InfluencerId = influencerId;
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpPut("{platformId}")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Update([FromRoute] int platformId, [FromBody] UpdateInfluencerPlatformCommand command)
        {
            if (platformId != command.PlatformId)
            {
                return BadRequest();
            }
            command.InfluencerId = influencerId;
            return Ok(await Mediator.Send(command));
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            DeleteInfluencerPlatformCommand command = new DeleteInfluencerPlatformCommand
            {
                Id = id,
                InfluencerId = influencerId
            };
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("{id}/verify")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> RequestVerify([FromRoute] int id, [FromBody] VerifyInfluencerPlatformCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.InfluencerId = influencerId;
            return Ok(await Mediator.Send(command));
        }
    }
}
