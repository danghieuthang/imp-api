using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMP.Application.Models.File
{
    public class UploadFileRequest
    {
        /// <summary>
        /// The file upload
        /// </summary>
        /// <value></value>
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
        /// <summary>
        /// List subfolder. Example: page/block/item
        /// </summary>
        /// <value>A folder</value>
        [FromForm(Name = "subfolders")]
        public List<string> Subfolders { get; set; }
    }
}