//using System.Threading.Tasks;
//using IMP.Application.Features.BlockCampaigns.Commands.CreateBlockCampaign;
//using IMP.Application.Features.BlockCampaigns.Commands.DeleteBlockCampaign;
//using IMP.Application.Features.BlockCampaigns.Commands.UpdateBlockCampaign;
//using IMP.Application.Interfaces;
//using IMP.Application.Models.ViewModels;
//using IMP.Application.Wrappers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace IMP.WebApi.Controllers.v1
//{
//    [ApiVersion("1.0")]
//    [Route(RouterConstants.BlockCampaign)]
//    [Authorize]
//    public class BlockCampaignController : BaseApiController
//    {
//        private readonly int _appId;
//        public BlockCampaignController(IAuthenticatedUserService authenticatedUserService)
//        {
//            _appId = 0;
//            int.TryParse(authenticatedUserService.AppId, out _appId);
//        }

//        /// <summary>
//        ///  Create new a block campaign
//        /// </summary>
//        /// <param name="command">The Create Block Campaign Command</param>
//        /// <returns></returns>
//        [ProducesResponseType(typeof(Response<BlockCampaignViewModel>), 201)]
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateBlockCampaignCommand command)
//        {
//            command.InfluencerId = _appId;
//            return StatusCode(201, await Mediator.Send(command));
//        }

//        /// <summary>
//        /// Update a block campaign
//        /// </summary>
//        /// <param name="id">The id of campaign</param>
//        /// <param name="command">The Update Block Campaign Command</param>
//        /// <returns></returns>
//        [ProducesResponseType(typeof(Response<BlockCampaignViewModel>), 200)]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBlockCampaignCommand command)
//        {
//            if (id != command.Id)
//            {
//                return BadRequest();
//            }
//            command.InfluencerId = _appId;
//            return Ok(await Mediator.Send(command));
//        }

//        /// <summary>
//        /// Delete a block campaign by id
//        /// </summary>
//        /// <param name="id">The block campaign id</param>
//        /// <returns></returns>
//        [ProducesResponseType(typeof(Response<int>), 200)]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete([FromRoute] int id)
//        {

//            var command = new DeleteBlockCampaignCommand
//            {
//                Id = id,
//                InfluencerId = _appId
//            };
//            return Ok(await Mediator.Send(command));
//        }
//    }
//}