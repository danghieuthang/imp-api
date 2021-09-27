using IMP.Application.Features.Banks.Queries.GetAllBank;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Bank)]
    public class BankController : BaseApiController
    {
        /// <summary>
        /// Get list banking
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<BankViewModel>>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllBankQuery()));
        }
    }
}
