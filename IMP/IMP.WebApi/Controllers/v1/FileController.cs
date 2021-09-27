using System.Net;
using System.Threading.Tasks;
using IMP.Application.Interfaces;
using IMP.Application.Models.File;
using IMP.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMP.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route(RouterConstants.File)]
    [Authorize]
    public class FileController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authetincatedUserService;
        private readonly IFileService _fileService;

        public FileController(IAuthenticatedUserService authetincatedUserService, IFileService fileService)
        {
            _authetincatedUserService = authetincatedUserService;
            _fileService = fileService;
        }

        /// <summary>
        /// Upload a image to 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<UploadFileResponse>), 200)]
        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadFileRequest request)
        {
            return StatusCode(200, await _fileService.UploadImage(_authetincatedUserService.AppId, request));
        }

        /// <summary>
        /// Upload a video to 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(Response<UploadFileResponse>), 200)]
        [HttpPost("video")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadFileRequest request)
        {
            return StatusCode(200, await _fileService.UploadVideo(_authetincatedUserService.AppId, request));
        }
    }
}