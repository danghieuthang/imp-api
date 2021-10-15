using IMP.Application.Features.Wallets.Commands.RequestCharge;
using IMP.Application.Features.Wallets.Commands.RequestWithdraw;
using IMP.Application.Features.Wallets.Queries.GetWalletByUserId;
using IMP.Application.Features.WalletTransactions.Queries.GetTransactionsByWalletId;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Wallet)]
    [Authorize]
    public class WalletController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly int _appId;

        public WalletController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
            _appId = 0;
            _ = int.TryParse(_authenticatedUserService.AppId, out _appId);
        }

        /// <summary>
        /// Get wallet of authenticated user
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<WalletViewModel>), (int)HttpStatusCode.OK)]

        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetWalletByUserIdQuery { ApplicationUserId = _appId }));
        }

        /// <summary>
        /// Get wallet by id
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<WalletViewModel>), (int)HttpStatusCode.OK)]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalletById([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetWalletByUserIdQuery { ApplicationUserId = _appId, WalletId = id }));
        }



        /// <summary>
        /// Query transaction of authenticated user
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<WalletTransactionViewModel>>), (int)HttpStatusCode.OK)]
        [HttpGet("me/transactions")]
        public async Task<IActionResult> Get([FromQuery] GetWalletTransactionByWalletIdQuery query)
        {
            query.SetApplicationUserId(_appId);
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get transaction of wallet id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<WalletTransactionViewModel>>), (int)HttpStatusCode.OK)]
        [HttpGet("{id}/transactions")]
        public async Task<IActionResult> GetTransactionOfWallet(int id, [FromQuery] GetWalletTransactionByWalletIdQuery query)
        {
            query.SetApplicationUserId(_appId);
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Request charge money to wallet for authenticated user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<ChargeWalletResponse>), (int)HttpStatusCode.OK)]
        [HttpPost("me/request-charge")]
        public async Task<IActionResult> RequestCharge([FromBody] RequestChargeWalletCommand command)
        {
            command.ApplicationUserId = _appId;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Request withdraw money from wallet for authenticated user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<WalletTransactionViewModel>), (int)HttpStatusCode.OK)]
        [HttpPost("me/request-withdraw")]
        public async Task<IActionResult> RequestWithDraw([FromBody] RequestWithdrawWalletCommand command)
        {
            command.ApplicationUserId = _appId;
            return Ok(await Mediator.Send(command));
        }
    }
}
