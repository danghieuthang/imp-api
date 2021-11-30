using IMP.Application.Features.Notifications.Queries;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMP.Application.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IMP.Application.Features.Notifications.Commands.DeleteNotification;
using IMP.Application.Features.Notifications.Commands.MakeNotificationRead;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.Notification)]
    [ApiVersion("1.0")]
    public class NotificationController : BaseApiController
    {
        /// <summary>
        /// Get notification of authenticated user
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<IEnumerable<NotificationViewModel>>), 200)]
        [Authorize]
        public async Task<IActionResult> GetNotification([FromQuery] GetListNotificationOfUserQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        /// <summary>
        /// Count number notification of authenticate user
        /// </summary>
        /// <returns></returns>
        [HttpGet("count-unread")]
        [ProducesResponseType(typeof(Response<int>), 200)]
        [Authorize]
        public async Task<IActionResult> CountUnreadNotification()
        {
            return Ok(await Mediator.Send(new GetNumberOfUnreadNotificationQuery()));
        }


        /// <summary>
        /// Delete a notification by id(only for owner of this notification)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteNotificationByIdCommand { Id = id }));
        }

        /// <summary>
        /// Get notification by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<NotificationViewModel>), 200)]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetNotificationByIdQuery { Id = id }));
        }

        /// <summary>
        /// Make a notification as read
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/read")]
        [ProducesResponseType(typeof(Response<NotificationViewModel>), 200)]
        [Authorize]
        public async Task<IActionResult> Read([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new MakeNotificationIsReadCommand { Id = id }));
        }

        /// <summary>
        /// Make all notification of authenticated user as read
        /// </summary>
        /// <returns></returns>
        [HttpPut("make-all-read")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize]
        public async Task<IActionResult> MakeAllkRead()
        {
            return Ok(await Mediator.Send(new MakeAllNotificationAsReadCommand()));
        }

    }
}
