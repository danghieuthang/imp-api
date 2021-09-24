using IMP.Application.Features.Wallets.Commands.RequestCharge;
using IMP.Application.Features.Wallets.Queries.GetWalletByUserId;
using IMP.Application.Features.WalletTransactions.Queries.GetTransactionsByWalletId;
using IMP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetWalletByUserIdQuery { ApplicationUserId = _appId }));
        }

        [HttpGet("me/transactions")]
        public async Task<IActionResult> Get([FromQuery] GetWalletTransactionByWalletIdQuery query)
        {
            query.SetApplicationUserId(_appId);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("me/request-charge")]
        public async Task<IActionResult> RequestCharge([FromBody] RequestChargeWalletCommand command)
        {
            command.ApplicationUserId = _appId;
            return Ok(await Mediator.Send(command));
        }
    }
}
