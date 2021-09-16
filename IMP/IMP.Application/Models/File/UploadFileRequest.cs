using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace IMP.Application.Models.File
{
    public class UploadFileRequest
    {
        /// <summary>
        /// The file upload
        /// </summary>
        /// <value></value>
        public IFormFile File { get; set; }
        /// <summary>
        /// List subfolder. Example: page/block/item
        /// </summary>
        /// <value>A folder</value>
        public List<string> Subfolders { get; set; }
    }
}