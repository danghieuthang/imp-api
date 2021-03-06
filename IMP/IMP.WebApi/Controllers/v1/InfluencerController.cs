using IMP.Application.Features.ApplicationUsers.Queries.GetAllInfluencer;
using IMP.Application.Features.ApplicationUsers.Queries.GetInfluencerById;
using IMP.Application.Features.Campaigns.Queries.GetCampaignByInfluencerId;
using IMP.Application.Features.Influencers.Commands;
using IMP.Application.Features.Influencers.Queries;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.Influencer)]
    [ApiVersion("1.0")]
    public class InfluencerController : BaseApiController
    {
        /// <summary>
        /// Get influencer by id
        /// </summary>
        /// <param name="id">The Id of influencer.</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Response<InfluencerViewModel>), 200)]
        public async Task<IActionResult> GetInfluencerById([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetInfluencerByIdCommand { Id = id }));
        }

        /// <summary>
        /// Search influencer 
        /// </summary>
        /// <param name="query">The Search Influencer Query</param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(Response<PagedList<InfluencerViewModel>>), 200)]
        public async Task<IActionResult> SearchInfluencer([FromQuery] GetAllInfluencerQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Search influencer suitable with campaign
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search-influencer-suitable-with-campaign")]
        [ProducesResponseType(typeof(Response<PagedList<InfluencerViewModel>>), 200)]
        public async Task<IActionResult> SearchInfluencerSutableWithCampaign([FromQuery] GetAllInfluencerSuitableByCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get influencer by nickname
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        [HttpGet("{nickname}")]
        [ProducesResponseType(typeof(Response<PagedList<InfluencerViewModel>>), 200)]
        public async Task<IActionResult> SearchInfluencer([FromRoute] string nickname)
        {
            return Ok(await Mediator.Send(new GetInfluencerByIdCommand { Nickname = nickname }));
        }

        /// <summary>
        /// Add invited a influencer join to campaign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/invite-join-campaign")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> InviteInfluencer([FromRoute] int id, [FromBody] InviteInfluencerToCampaignCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Search list campaign of a influencer
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("{id}/campaigns")]
        [ProducesResponseType(typeof(Response<IPagedList<CampaignViewModel>>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> InviteInfluencer([FromQuery] GetCampaignByInfluencerIdForBrandQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// GEt status of authenticated influencer
        /// </summary>
        /// <returns></returns>
        [HttpGet("me/status")]
        [ProducesResponseType(typeof(Response<int>), 200)]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetStatusOfAuthenticatedUser()
        {
            return Ok(await Mediator.Send(new GetInfluencerStatusQuery()));
        }


    }
}
