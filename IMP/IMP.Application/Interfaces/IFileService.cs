using System.Threading.Tasks;
using IMP.Application.Models.File;

namespace IMP.Application.Interfaces
{
    public interface IFileService
    {
        Task<UploadFileResponse> UploadImage(string user, UploadFileRequest request);
        Task<UploadFileResponse> UploadVideo(string user, UploadFileRequest request);
    }
}