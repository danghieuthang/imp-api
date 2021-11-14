using IMP.Application.Features.EvidenceTypes.Queries.GetAllEvidenceType;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.EvidenceType)]
    [ApiVersion("1.0")]
    public class EvidenceTypeController : BaseApiController
    {
        /// <summary>
        /// Get all evidence types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<EvidenceTypeViewModel>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllEvidenceTypeQuery()));
        }
    }
}
