using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IFirebaseService
    {
        // <summary>
        /// The file stream to firebase storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName">The file name of file</param>
        /// <param name="subfolders">Subfoders</param>
        /// <returns>The file url</returns>
        Task<string> UploadFile(Stream stream, string fileName, params string[] subfolders);
    }
}
