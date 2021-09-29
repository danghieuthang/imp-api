using IMP.Application.Features.WalletTransactions.Commands;
using IMP.Application.Features.WalletTransactions.Commands.CancelWalletTransaction;
using IMP.Application.Features.WalletTransactions.Queries.GetAllTransactions;
using IMP.Application.Features.WalletTransactions.Commands.ConfirmVnpWalletTransaction;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using IMP.Application.Features.WalletTransactions.Commands.CreateWalletTransaction;
using IMP.Application.Features.WalletTransactions.Commands.ProcessWalletTransaction;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.WalletTransaction)]
    public class WalletTransactionController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public WalletTransactionController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        /// <summary>
        /// API để VNPAY gọi ngược lại khi giao dịch thành công.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet("confirm-transaction-vnpay")]
        public async Task<IActionResult> ConfirmWalletTransaction([FromQuery] ConfirmVnpWalletTransactionCommand command)
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

        /// <summary>
        /// Confirm a transaction successfull has status is Processing. Only for Administrator
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}/confirm-successfully")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(Response<CompletedWalletTransactionCommand>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CompletedWalletTransactionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.AdminId = _authenticatedUserService.ApplicationUserId;
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Change status of new transaction to processing. This transaction will lock by admin requet processing
        /// </summary>
        /// <param name="id">The id of transaction.</param>
        /// <returns></returns>
        [HttpPut("{id}/processing")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(Response<CompletedWalletTransactionCommand>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var command = new ProcessWalletTransactionCommand { Id = id, AdminId = _authenticatedUserService.ApplicationUserId };
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Cancel a transaction has status is Processing. Only for Administrator or owner of transaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/cancel")]
        [Authorize]
        [ProducesResponseType(typeof(Response<WalletTransactionViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CancelWalletTransactionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            // if not admin
            if (_authenticatedUserService.IsAdmin.HasValue && !_authenticatedUserService.IsAdmin.Value)
            {
                command.ApplicationUserId = _authenticatedUserService.ApplicationUserId;
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Create transaction transfer money from wallet to another wallet
        /// </summary>
        /// <param name="command">The Create Wallet Transaction Command.</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<WalletTransactionViewModel>), 201)]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateWalletTransactionCommand command)
        {
            command.ApplicationUserFrom = _authenticatedUserService.ApplicationUserId;
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
