using IMP.Application.Features.VoucherTransactions.Commands.CreateVoucherTransaction;
using IMP.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactionOfCampaign;
using IMP.Application.Features.VoucherTransactions.Queries.GetTransactionById;
using IMP.Application.Features.VoucherTransactions.Queries.GetTransactionReportOfCampaign;
using IMP.Application.Features.VoucherTransactions.Queries.GetTransactionReportOfVoucher;
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
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateVoucherTransactionCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }
        /// <summary>
        /// Search transaction of campaign
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search-transaction-of-campaign")]
        [ProducesResponseType(typeof(Response<IPagedList<VoucherTransactionViewModel>>), 200)]
        public async Task<IActionResult> GetAllOfCampaign([FromQuery] GetAllVoucherTransactionOfCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Search transaction of a voucher by member id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search-transaction-of-voucher-by-member")]
        [ProducesResponseType(typeof(Response<IPagedList<VoucherTransactionViewModel>>), 200)]
        public async Task<IActionResult> SearchTransactionOfVoucherByMember([FromQuery] GetAllVoucherTransactionOfVoucherQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get detail of voucher transaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<VoucherTransactionViewModel>), 200)]
        [Authorize(Roles = "Brand,Influencer")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetTransactionByIdQuery { Id = id }));
        }

        [HttpGet("report")]
        [ProducesResponseType(typeof(Response<VoucherTransactionReportViewModel>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> GetAllOfCampaign([FromQuery(Name = "campaign_id")] int campaignId)
        {
            return Ok(await Mediator.Send(new GetTransactionReportOfCampaignQuery { CampaignId = campaignId }));
        }

        /// <summary>
        /// Get report of a voucher code by member id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("report-of-voucher-by-member")]
        [ProducesResponseType(typeof(Response<VoucherTransactionReportOfVoucherViewModel>), 200)]
        [Authorize(Roles = "Brand,Influencer")]
        public async Task<IActionResult> ReportOfVoucherbyMember([FromQuery] TransactionReportOfVoucherQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
