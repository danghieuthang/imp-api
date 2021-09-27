using IMP.Application.Features.WalletTransactions.Commands;
using IMP.Application.Features.WalletTransactions.Queries.GetAllTransactions;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

        /// <summary>
        /// Query wallet transactions. Only for Administrator
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet()]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(Response<IPagedList<WalletTransactionViewModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] GetAllTransactionsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
