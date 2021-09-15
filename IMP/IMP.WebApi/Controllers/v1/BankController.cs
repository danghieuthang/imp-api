using IMP.Application.Features.Banks.Queries.GetAllBank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.BANK)]
    public class BankController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllBankQuery()));
        }
    }
}
