using IMP.Application.Features.Locations.Queries.GetAllLocations;
using IMP.Application.Features.Locations.Queries.GetLocationByCode;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.Location)]
    public class LocationController : BaseApiController
    {
        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<IEnumerable<LocationViewModel>>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllLocationQuery()));
        }

        /// <summary>
        /// Get location by code
        /// </summary>
        /// <param name="code">The code of location</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<LocationViewModel>), 200)]
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            return Ok(await Mediator.Send(new GetLocationByCodeQuery { Code = code }));
        }
    }
}
