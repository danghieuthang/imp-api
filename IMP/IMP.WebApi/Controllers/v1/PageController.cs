using System.Threading.Tasks;
using IMP.Application.Features.Blocks.Queries;
using IMP.Application.Features.Pages.Commands.CreatePage;
using IMP.Application.Features.Pages.Commands.DeletePage;
using IMP.Application.Features.Pages.Commands.UpdatePage;
using IMP.Application.Features.Pages.Queries.GetAllPageOfInfluencer;
using IMP.Application.Interfaces;
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

        [HttpGet("me")]
        public async Task<IActionResult> Get()
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            return Ok(await Mediator.Send(new GetAllPageOfInfluencerQuery { InfluencerId = id }));
        }

        [HttpGet("{id}/blocks")]
        public async Task<IActionResult> GetBlockByPage([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new GetAllBlockByPageIdQuery { PageId = id }));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePageCommand command)
        {
            int id = 0;
            int.TryParse(_authenticatedUserService.AppId, out id);
            command.InfluencerId = id;
            return StatusCode(201, await Mediator.Send(command));
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            int influencerId = 0;
            int.TryParse(_authenticatedUserService.AppId, out influencerId);
            return Ok(await Mediator.Send(new DeletePageCommand { Id = id, InfluencerId = influencerId }));
        }
    }
}