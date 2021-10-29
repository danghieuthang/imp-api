using System.Threading.Tasks;
using IMP.Application.Features.Vouchers.Commands.CreateVoucher;
using IMP.Application.Features.Vouchers.Commands.DeleteVoucher;
using IMP.Application.Features.Vouchers.Commands.UpdateVoucher;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Voucher)]
    [Authorize(Roles = "Brand")]
    public class VoucherController : BaseApiController
    {
        private readonly int _appId;
        public VoucherController(IAuthenticatedUserService authenticatedUserService)
        {
            _appId = 0;
            int.TryParse(authenticatedUserService.AppId, out _appId);
        }
        /// <summary>
        /// Create a voucher
        /// </summary>
        /// <param name="command">The Create Voucher Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherViewModel>), 201)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVoucherCommand command)
        {
            command.ApplicationUserId = _appId;
            return StatusCode(201, await Mediator.Send(command));
        }
        /// <summary>
        /// Update a voucher
        /// </summary>
        /// <param name="id">The voucher id</param>
        /// <param name="command">The Update Voucher Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherViewModel>), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVoucherCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.ApplicationUserId = _appId;
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Delete a voucher by id
        /// </summary>
        /// <param name="id">The voucher id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteVoucherCommand
            {
                Id = id,
                BrandId = _appId
            };

            return Ok(await Mediator.Send(command));
        }
    }
}