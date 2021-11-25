﻿using IMP.Application.Features.Notifications.Queries;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMP.Application.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IMP.Application.Features.Notifications.Commands.DeleteNotification;

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
        /// Delete a notification by id(only for owner of this notification)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteNotificationByIdCommand { Id=id}));
        }

    }
}