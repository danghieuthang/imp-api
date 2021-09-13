using IMP.Application.Features.Locations.Queries.GetAllLocations;
using IMP.Application.Features.Locations.Queries.GetLocationByCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.LOCATION)]
    public class LocationController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllLocationQuery()));
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            return Ok(await Mediator.Send(new GetLocationByCodeQuery { Code = code }));
        }
    }
}
