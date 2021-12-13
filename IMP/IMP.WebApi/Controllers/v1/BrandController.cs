using IMP.Application.Features.Brands.Commands.GenerateNewSercretKey;
using IMP.Application.Features.Brands.Commands.ProcessBrand;
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
    public class BrandController : BaseApiController
    {
        public BrandController()
        {
        }
        /// <summary>
        /// Get brand of user who authenticated
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(Response<BrandFullViewModel>), 200)]
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetBrandByApplicationUserIdQuery { }));
        }

        /// <summary>
        /// Search all brand
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<PagedList<BrandViewModel>>), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SearchBrandsQuery query)
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
        [Authorize(Roles = "Brand")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBrandCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Generate new secret key
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<string>), 200)]
        [Authorize(Roles = "Brand")]
        [HttpPost("me/generate-secret-key")]
        public async Task<IActionResult> GenerateScretKey([FromBody] GenerateNewSecretKeyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Enable brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/enable")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize(Roles = "Admininstrator")]
        public async Task<IActionResult> Enable([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new EnableBrandCommand { Id = id }));
        }

        /// <summary>
        /// Disblae brand
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/disable")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [Authorize(Roles = "Admininstrator")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            return Ok(await Mediator.Send(new DisableBrandCommand { Id = id }));
        }
    }
}
