//using IMP.Application.Features.Milestones.Queries.GetAllMilestones;
//using IMP.Application.Models.Compaign;
//using IMP.Application.Wrappers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace IMP.WebApi.Controllers.v1
//{
//    [Route(RouterConstants.Milestones)]
//    [ApiVersion("1.0")]
//    public class MilestoneController : BaseApiController
//    {
//        /// <summary>
//        /// Get all milestones
//        /// </summary>
//        /// <returns></returns>
//        [ProducesResponseType(typeof(Response<IEnumerable<MilestoneViewModel>>), 200)]
//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            return Ok(await Mediator.Send(new GetAllMilestonesQuery()));
//        }
//    }
//}
