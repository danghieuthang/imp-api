using IMP.Application.Features.ApplicationUsers.Queries.GetAllInfluencer;
using IMP.Application.Features.ApplicationUsers.Queries.GetInfluencerById;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
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
        /// Get influencer information by id
        /// </summary>
        /// <param name="id">The Id of influencer.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
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

    }
}
