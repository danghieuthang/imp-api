using IMP.Application.Features.Evidences.Commands.CreateEvidence;
using IMP.Application.Features.Evidences.Commands.DeleteEvidence;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.Evidence)]
    [ApiVersion("1.0")]
    [Authorize()]
    public class EvidenceController : BaseApiController
    {
        /// <summary>
        /// Create evidence
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Response<EvidenceViewModel>), 201)]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Create([FromBody] CreateEvidenceCommand command)
        {
            return StatusCode(201, await Mediator.Send(command));
        }

        /// <summary>
        /// Delete evidence
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Response<int>), 200)]
        [Authorize(Roles = "Influencer")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DeleteEvidenceCommand { Id = id }));
        }
    }
}
