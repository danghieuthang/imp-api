using IMP.Application.Features.VoucherTransactions.Commands.CreateVoucherTransaction;
using IMP.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactionOfCampaign;
using IMP.Application.Features.VoucherTransactions.Queries.GetTransactionReportOfCampaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.VoucherTranscation)]
    [ApiVersion("1.0")]
    public class VoucherTransactionController : BaseApiController
    {
        /// <summary>
        /// Create a voucher transaction of voucher code
        /// </summary>
        /// <param name="command">The Create Voucher Transaction Command</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response<VoucherTransactionViewModel>), 201)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Create([FromBody] CreateVoucherTransactionCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }
        /// <summary>
        /// Search transction of campaign
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search-transaction-of-campaign")]
        [ProducesResponseType(typeof(Response<IPagedList<VoucherTransactionViewModel>>), 200)]
        public async Task<IActionResult> GetAllOfCampaign([FromQuery] GetAllVoucherTransactionOfCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpGet("report")]
        [ProducesResponseType(typeof(Response<VoucherTransactionReportViewModel>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> GetAllOfCampaign([FromQuery(Name = "campaign_id")] int campaignId)
        {
            return Ok(await Mediator.Send(new GetTransactionReportOfCampaignQuery { CampaignId = campaignId }));
        }
    }
}
