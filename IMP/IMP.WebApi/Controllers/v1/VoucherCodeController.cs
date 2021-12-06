using IMP.Application.Features.VoucherCodes.Commands.AssignVoucherCodeForCampaignMember;
using IMP.Application.Features.VoucherCodes.Commands.CreateVoucherCode;
using IMP.Application.Features.VoucherCodes.Commands.RequestVoucherCode;
using IMP.Application.Features.VoucherCodes.Commands.UnAssignVoucherCodeForMember;
using IMP.Application.Features.VoucherCodes.Commands.UpdateVoucherCode;
using IMP.Application.Features.VoucherCodes.DeleteVoucherCode;
using IMP.Application.Features.VoucherCodes.Queries.GetVoucherCodeFromEncryptData;
using IMP.Application.Features.Vouchers.Queries.GetAllVoucherByApplicationUser;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.VoucherCode)]
    [ApiVersion("1.0")]
    [Authorize()]
    public class VoucherCodeController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public VoucherCodeController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }
        /// <summary>
        /// Create a voucher code for voucher
        /// </summary>
        /// <param name="command">The Create Voucher Code Request Model</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherCodeViewModel>), 201)]
        [HttpPost]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Create([FromBody] CreateVoucherCodeCommand command)
        {
            command.ApplicationUserId = _authenticatedUserService.ApplicationUserId;
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Delete voucher code by id
        /// </summary>
        /// <param name="id">The id of voucher code</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteVoucherCodeCommand { Id = id, ApplicationUserId = _authenticatedUserService.ApplicationUserId }));
        }

        /// <summary>
        /// Update voucher code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<VoucherCodeViewModel>), 200)]
        [HttpPut("{id}")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateVoucherCodeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Assign a voucher code for campaign member
        /// </summary>
        /// <param name="id">The id of voucher code</param>
        /// <param name="command">The command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPut("{id}/assign-for-member")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> AssignVoucherCodeForCampaignMember([FromRoute] int id, [FromBody] AssignVoucherCodeForCampaignMemberCommand command)
        {
            if (id != command.VoucherCodeId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Un assign voucher code for campaign member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPut("{id}/unassign-for-member")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> AssignVoucherCodeForCampaignMember([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new UnassignVoucherCodeForMemberCommand { Id = id }));
        }

        /// <summary>
        /// Asssign multiple voucher code for all campaign member of campaign
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPut("assign-for-members")]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> AssignVoucherCodeForCampaignMember([FromBody] AssignVoucherCodesForCampaignCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Take voucher code from biolink and add to authenticated user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPost("take-voucher-code-from-biolink")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> RequestVoucherCodeFromBioLink([FromBody] RequestVoucherCodeByBiolinkAndCampaignCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Get voucher code and send to email by biolink
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [HttpPost("take-voucher-code-to-email-from-biolink")]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> RequestVoucherCodeToEmailFrombioLink([FromBody] RequestVoucherCodeToEmailByBiolinkCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Checking voucher code from encrypt data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<CheckVoucherCodeViewModel>), 200)]
        [HttpPost("checking-voucher-code")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckingVoucherFromEncryptData([FromQuery] GetVoucherCodeFromEncryptDataQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


    }
}
