using IMP.Application.Enums;
using IMP.Application.Features.MemberActivities.Commands.ChangeMemberActivityStatus;
using IMP.Application.Features.MemberActivities.Queries.GetAllActivity;
using IMP.Application.Features.MemberActivities.Queries.GetMemberActivityById;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.MemberActivity)]
    [ApiVersion("1.0")]
    public class MemberActivityController : BaseApiController
    {
        /// <summary>
        /// Get detail a member activity(with list evidence and comments)
        /// </summary>
        /// <param name="id">The id of member activity</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<List<MemberActivityViewModel>>), 200)]
        public async Task<IActionResult> GetMemberActivityById([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetMemberActivityByIdQuery { Id = id }));
        }

        /// <summary>
        /// Accept a member activity
        /// </summary>
        /// <param name="id">The id of member activity</param>
        /// <returns></returns>
        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Brand")]
        [ProducesResponseType(typeof(MemberActivityViewModel), 200)]
        public async Task<IActionResult> Accept([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ChangeMemberActivityStatusCommand { Id = id, Status = MemberActivityStatus.Completed }));
        }


        /// <summary>
        /// Reject a member activity
        /// </summary>
        /// <param name="id">The id of member activity</param>
        /// <returns></returns>
        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Brand")]
        [ProducesResponseType(typeof(MemberActivityViewModel), 200)]
        public async Task<IActionResult> Reject([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ChangeMemberActivityStatusCommand { Id = id, Status = MemberActivityStatus.UnComleted }));
        }

        [HttpPut("{id}/pending")]
        [Authorize(Roles = "Influencer")]
        [ProducesResponseType(typeof(MemberActivityViewModel), 200)]
        public async Task<IActionResult> Pending([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ChangeMemberActivityStatusCommand { Id = id, Status = MemberActivityStatus.Waiting }));
        }

        /// <summary>
        /// Search all member activity of brand who are logged in
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search-all-activity-of-brand")]
        [ProducesResponseType(typeof(IPagedList<MemberActivityViewModel>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> SearchAllMemberAcitivity([FromQuery] GetAllActivityOfBrandQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
