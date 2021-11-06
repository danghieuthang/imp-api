using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route(RouterConstants.Job)]
    [ApiVersion("1.0")]
    public class JobController : BaseApiController
    {
        [ProducesResponseType(typeof(Response<List<string>>), 200)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = new Response<List<string>>
            {
                Data = new List<string>
                {
                    "Học Sinh",
                    "Công nghệ thông tin",
                    "Nhiếp ảnh gia",
                    "Giáo viên"
                }
            };
            return Ok(response);
        }
    }
}
