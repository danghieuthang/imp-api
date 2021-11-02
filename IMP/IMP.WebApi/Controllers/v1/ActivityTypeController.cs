using IMP.Application.Features.ActivityTypes.Commands.CreateActivityType;
using IMP.Application.Features.ActivityTypes.Queries.GetAllActivityTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.ActivityType)]
    public class ActivityTypeController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateActivityTypeCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllActivityTypesQuery()));
        }
    }
}
