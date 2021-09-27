using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IMP.WebApi.Controllers
{
    public class MetaController : BaseApiController
    {
        /// <summary>
        /// Check api info
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }
    }
}
