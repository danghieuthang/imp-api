using System.Threading.Tasks;
using IMP.Application.Models.File;
using IMP.Application.Wrappers;

namespace IMP.Application.Interfaces
{
    public interface IFileService
    {
        Task<Response<UploadFileResponse>> UploadImage(string user, UploadFileRequest request);
        Task<Response<UploadFileResponse>> UploadVideo(string user, UploadFileRequest request);
    }
}