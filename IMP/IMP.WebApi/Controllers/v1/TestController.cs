using IMP.Application.Interfaces.Shared;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IMP.WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IZaloService _zaloService;

        public TestController(IZaloService zaloService)
        {
            _zaloService = zaloService;
        }

        [HttpPost("test-zelo")]
        public async Task<IActionResult> SendMessageZalo([FromBody] string clientToken)
        {
            _ = await _zaloService.SendMessageAsync(clientToken);
            return Ok();
        }

        [HttpGet("login-url")]
        public async Task<IActionResult> SendMessageZalo()
        {
            string url = await _zaloService.GetLoginUrlAsync("https://developers.zalo.me/docs/sdk/dotnet-sdk/tai-lieu/social-api-post-4331");
            return Ok(new Response<string>(data: url));
        }
    }
}
