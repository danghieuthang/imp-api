using IMP.Application.Enums;
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
    }
}
