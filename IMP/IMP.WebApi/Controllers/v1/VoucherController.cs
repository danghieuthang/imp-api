using System.Threading.Tasks;
using IMP.Application.Features.Vouchers.Commands.CreateVoucher;
using IMP.Application.Features.Vouchers.Commands.DeleteVoucher;
using IMP.Application.Features.Vouchers.Commands.UpdateVoucher;
using IMP.Application.Interfaces;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVoucherCommand command)
        {
            command.BrandId = _appId;
            return StatusCode(201, await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVoucherCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.BrandId = _appId;
            return Ok(await Mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Create([FromRoute] int id)
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