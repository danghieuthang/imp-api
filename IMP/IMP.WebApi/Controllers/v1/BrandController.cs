using IMP.Application.Features.Brands.Commands.UpdateBrand;
using IMP.Application.Features.Brands.Queries;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Brand)]
    [Authorize]
    public class BrandController : BaseApiController
    {
        private IAuthenticatedUserService _authenticatedUserService;
        public BrandController(IAuthenticatedUserService authenticatedUser)
        {
            _authenticatedUserService = authenticatedUser;
        }
        /// <summary>
        /// Get brand of user who authenticated
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(Response<BrandViewModel>), 200)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetBrandByApplicationUserIdQuery { ApplicationUserId = _authenticatedUserService.ApplicationUserId }));
        }

        /// <summary>
        /// Search all brand
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<PagedList<BrandViewModel>>), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery]SearchBrandsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        /// <summary>
        /// Update a brand
        /// </summary>
        /// <param name="id">The Id of brand</param>
        /// <param name="command">The Update Brand Command</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Response<BrandViewModel>), 200)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBrandCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            command.ApplicationUserId = _authenticatedUserService.ApplicationUserId;
            return Ok(await Mediator.Send(command));
        }
    }
}
