using IMP.Application.Features.RankLevels.Queries.GetAllRankLevel;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.RankLevel)]
    public class RankLevelController : BaseApiController
    {
        /// <summary>
        /// Get all rank level
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<RankLevelViewModel>>), 200)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllRankLevelQuery()));
        }
    }
}
