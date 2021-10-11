using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMP.Application.Features.Blocks.Queries;
using IMP.Application.Features.Pages.Commands.CreatePage;
using IMP.Application.Features.Pages.Commands.DeletePage;
using IMP.Application.Features.Pages.Commands.UpdatePage;
using IMP.Application.Features.Pages.Queries.GetAllPageOfInfluencer;
using IMP.Application.Features.Pages.Queries.GetPageById;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Page)]
    [Authorize]
    public class PageController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public PageController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        /// <summary>
        /// Get all page of authenticated user
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<PageViewModel>>), 200)]
        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            return Ok(await Mediator.Send(new GetAllPageOfInfluencerQuery { InfluencerId = id }));
        }

        /// <summary>
        /// Get page by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PageViewModel>), 200)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetPageByIdQuery
            {
                Id = id
            }));
        }
        /// <summary>
        /// Get page by bio link
        /// </summary>
        /// <param name="biolink">The bio link</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PageViewModel>), 200)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] string biolink)
        {
            return Ok(await Mediator.Send(new GetPageByIdQuery
            {
                BioLink = biolink
            }));
        }

        /// <summary>
        /// Get all block of page
        /// </summary>
        /// <param name="id">The page id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<BlockViewModel>>), 200)]
        [HttpGet("{id}/blocks")]
        public async Task<IActionResult> GetBlockByPage([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllBlockByPageIdQuery { PageId = id }));
        }

        /// <summary>
        /// Create a page
        /// </summary>
        /// <param name="command">The Create Page Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PageViewModel>), 201)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePageCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.InfluencerId = id;
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Update a page
        /// </summary>
        /// <param name="id">The page id</param>
        /// <param name="command">The Update Page Command</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<PageViewModel>), 200)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePageCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            int influencerId = 0;
            int.TryParse(_authenticatedUserService.AppId, out influencerId);
            command.InfluencerId = influencerId;
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a page
        /// </summary>
        /// <param name="id">The id of page</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<int>), 200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            int influencerId = 0;
            int.TryParse(_authenticatedUserService.AppId, out influencerId);
            return Ok(await Mediator.Send(new DeletePageCommand { Id = id, InfluencerId = influencerId }));
        }
    }
}