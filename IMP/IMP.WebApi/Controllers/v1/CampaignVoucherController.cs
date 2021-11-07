using IMP.Application.Features.Vouchers.Commands.ReceiverVoucher;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.CampaignVoucher)]
    [ApiVersion("1.0")]
    public class CampaignVoucherController : BaseApiController
    {
        /// <summary>
        /// Request voucher code(Create )
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherCodeViewModel>), 200)]
        [HttpPost("{id}/request-voucher")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> RequestVoucher([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new ReceiverVoucherCommand { CampaignVoucherId = id }));
        }
    }
}
