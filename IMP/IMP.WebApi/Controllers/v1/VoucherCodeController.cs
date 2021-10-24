﻿using IMP.Application.Features.VoucherCodes.Commands.CreateVoucherCode;
using IMP.Application.Features.VoucherCodes.DeleteVoucherCode;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.VoucherCode)]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Brand")]
    public class VoucherCodeController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public VoucherCodeController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }
        /// <summary>
        /// Create a voucher code for voucher
        /// </summary>
        /// <param name="command">The Create Voucher Code Request Model</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherCodeViewModel>), 201)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVoucherCodeCommand command)
        {
            command.ApplicationUserId = _authenticatedUserService.ApplicationUserId;
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Delete voucher code by id
        /// </summary>
        /// <param name="id">The id of voucher code</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteVoucherCodeCommand { Id = id, ApplicationUserId = _authenticatedUserService.ApplicationUserId }));
        }
    }
}