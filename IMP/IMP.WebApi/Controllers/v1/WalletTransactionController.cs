using IMP.Application.Features.WalletTransactions.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.WalletTransaction)]
    public class WalletTransactionController : BaseApiController
    {
        /// <summary>
        /// API để VNPAY gọi ngược lại khi giao dịch thành công.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet("confirm-transaction")]
        public async Task<IActionResult> ConfirmWalletTransaction([FromQuery] CreateWalletTransactionCommand command)
        {
            var walletTransactionView = await Mediator.Send(command);
            return Ok(walletTransactionView);
        }
    }
}
