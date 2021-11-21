using IMP.Application.Features.ActivityComments.Commands.CreateActivityComment;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.ActivityComment)]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Brand,Influencer")]
    public class ActivityCommentController : BaseApiController
    {
        /// <summary>
        /// Create activity comment
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(Response<ActivityCommentViewModel>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateActivityCommentCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
