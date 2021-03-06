using System.Threading.Tasks;
using IMP.Application.Features.Vouchers.Commands.AssignVoucherForCampaign;
using IMP.Application.Features.Vouchers.Commands.CreateVoucher;
using IMP.Application.Features.Vouchers.Commands.DeleteVoucher;
using IMP.Application.Features.Vouchers.Commands.ImportVoucherCodes;
using IMP.Application.Features.Vouchers.Commands.UpdateVoucher;
using IMP.Application.Features.Vouchers.Queries.GetAllVoucherByApplicationUser;
using IMP.Application.Features.Vouchers.Queries.GetVoucherById;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IO;
using System;
using IMP.Application.Features.Vouchers.Queries.GetAllVoucher;
using IMP.Application.Features.Vouchers.Commands.ImportVouchers;
using IMP.Application.Features.Vouchers.Queries.GetVoucherCanAvailableForCampaign;
using IMP.Application.Features.Vouchers.Queries.GetRangeOfVoucher;
using IMP.Application.Features.Vouchers.Commands.RemoveVoucherFromCampaign;
using System.Collections.Generic;
using IMP.Application.Features.VoucherCodes.Commands.UpdateVoucherCode;
using IMP.Application.Features.Vouchers.Queries.GetAvailableVoucherForCampaignMember;
using IMP.Application.Features.Vouchers.Queries.Reports;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Voucher)]
    [Authorize()]
    public class VoucherController : BaseApiController
    {
        public VoucherController()
        {

        }
        /// <summary>
        /// Create a voucher
        /// Voucher Type: Shipping - 0,        Entry_Order - 1,        Specific_Product - 2
        /// Discount Valua Type:  Fixed - 0, Percentage - 1
        /// </summary>
        /// <param name="command">The Create Voucher Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherViewModel>), 201)]
        [HttpPost]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Create([FromBody] CreateVoucherCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }
        /// <summary>
        /// Update a voucher
        /// </summary>
        /// <param name="id">The voucher id</param>
        /// <param name="command">The Update Voucher Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherViewModel>), 200)]
        [HttpPut("{id}")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVoucherCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Delete a voucher by id
        /// </summary>
        /// <param name="id">The voucher id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteVoucherCommand
            {
                Id = id
            };

            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Get detail of a voucher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isGetCampaignInfo">If true then include campaign in campaign voucher</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherViewModel>), 200)]
        [HttpGet("{id}")]
        [Authorize(Roles = "Brand,Influencer,Administrator")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromQuery(Name = "is_get_campaigninfo")] bool isGetCampaignInfo)
        {
            return Ok(await Mediator.Send(new GetVoucherByIdQuery { Id = id, IsGetCampaignInfo = isGetCampaignInfo }));
        }

        /// <summary>
        /// Assign a voucher for campaign
        /// </summary>
        /// <param name="id">The id of voucher</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CampaignVoucherViewModel>), 200)]
        [HttpPost("{id}/assign-to-campaign")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> AssignVoucherForCampaign([FromRoute] int id, [FromBody] AssignVoucherToCampaignCommand command)
        {
            if (id != command.VoucherId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Un assign voucher from campaign
        /// </summary>
        /// <param name="id">The id of voucher</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpDelete("{id}/unassign-from-campaign")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> UnAssignVoucherForCampaign([FromRoute] int id, [FromBody] RemoveVoucherFromCampaignCommand command)
        {
            if (id != command.VoucherId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Import voucher code for voucher
        /// </summary>
        /// <param name="id">The id of voucher</param>
        /// <param name="command">The Import voucher code command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPost("{id}/import-codes")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> ImportVoucherCodes([FromRoute] int id, [FromForm] ImportVoucherCodesCommand command)
        {
            if (id != command.VoucherId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Import vouchers
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPost("import-voucher")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> ImportVouchers([FromForm] ImportVoucherCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        ///  Update list voucher code of voucher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherViewModel>), 200)]
        [HttpPut("{id}/update-voucher-codes")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> UpdateVoucherCodes([FromRoute] int id, [FromBody] UpdateListVoucherCodeCommand command)
        {
            if (id != command.VoucherId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Download import codes template
        /// </summary>
        /// <returns></returns>
        [HttpGet("download-import-codes-template")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImportCodesTemplate()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "App_Datas", "ExcelTemplates", "import-voucher-template_new.xlsx");
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            var stream = await System.IO.File.ReadAllBytesAsync(path);

            return File(stream, "application/octet-stream", "import-voucher-template_new.xlsx");

        }

        /// <summary>
        /// Download import voucher template
        /// </summary>
        /// <returns></returns>
        [HttpGet("download-import-voucher-template")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImportVocucherTemplate()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "App_Datas", "ExcelTemplates", "import_voucher_template.xlsx");
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            var stream = await System.IO.File.ReadAllBytesAsync(path);

            return File(stream, "application/octet-stream", "import_voucher_template.xlsx");
        }

        /// <summary>
        /// Search voucher of authenticated user(customer)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<UserVoucherViewModel>>), 200)]
        [HttpGet("search-for-customer")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetAllVoucherOfCustomer([FromQuery] GetAllVoucherByCustomerQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Search voucher for authenticate user(influencer)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<UserVoucherViewModel>>), 200)]
        [HttpGet("search-for-influencer")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> GetAllVoucherOfInluencer([FromQuery] GetAllVoucherByInfluencerQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Search vouchers of brand
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<VoucherViewModel>>), 200)]
        [HttpGet]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> GetAllVoucherByBrand([FromQuery] GetAllVoucherQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        /// <summary>
        /// Get list available voucher for campaign member of campaign
        /// </summary>
        /// <param name="campaignId">The campaign id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<VoucherViewModel>>), 200)]
        [HttpGet("available-for-campaign-member")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVoucherAvailableForCampaign([FromQuery(Name = "campaign_id")] int campaignId)
        {
            return Ok(await Mediator.Send(new GetAvailableVoucherForCampaignMemberQuery { CampaignId = campaignId }));
        }

        /// <summary>
        /// Get list available voucher for campaign
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IPagedList<VoucherViewModel>>), 200)]
        [HttpGet("available-for-campaign")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> GetVoucherAvailableForCampaign([FromQuery] GetVoucherCanAvailableForCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Get range date for search
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<RangeVoucherDateViewModel>), 200)]
        [HttpGet("range-date")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> GetRangeVoucherDate()
        {
            return Ok(await Mediator.Send(new GetRangeOfVoucherQuery()));
        }

        /// <summary>
        /// Search list voucher by biolink
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<VoucherViewModel>>), 200)]
        [HttpGet("search-voucher-by-biolink")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVouchersByBioLink([FromQuery] GetVoucherForBioLinkOfCampaignQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        /// <summary>
        /// Get report of voucher code by bio link
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherReportViewModel>), 200)]
        [HttpGet("report")]
        [AllowAnonymous]
        public async Task<IActionResult> ReportOfVoucherByBiolink([FromQuery] GetReportVoucherByBiolinkQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

    }
}