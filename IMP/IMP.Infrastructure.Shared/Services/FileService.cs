using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.File;
using IMP.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using IMP.Application.Exceptions;

namespace IMP.Infrastructure.Shared.Services
{
    public class FileService : IFileService
    {
        private readonly IFirebaseService _firebaseService;
        private readonly FileSettings _imageSettings;

        public FileService(IFirebaseService firebaseService, IOptions<FileSettings> options)
        {
            _firebaseService = firebaseService;
            _imageSettings = options.Value;
        }

        private List<ValidationError> ValidateFile(IFormFile formFile)
        {
            List<ValidationError> errors = new();
            if (!_imageSettings.AllowTypes.Contains(formFile.ContentType))
            {
                errors.Add(new ValidationError(field: "file", message: "Kiểu dữ liệu không hợp lệ."));
            }
            if (formFile.Length > _imageSettings.MaximumSize)
            {
                errors.Add(new ValidationError(field: "file", message: $"Dung lượng ảnh không được quá {_imageSettings.MaximumSize}."));
            };
            return errors;
        }
        public async Task<UploadFileResponse> UploadImage(string user, UploadFileRequest request)
        {
            var errors = ValidateFile(request.File);
            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
            request.Subfolders.Insert(0, user);
            string url = await _firebaseService.UploadFile(request.File.OpenReadStream(), request.File.FileName, request.Subfolders.ToArray());
            return new UploadFileResponse(url);
        }

        public Task<UploadFileResponse> UploadVideo(string user, UploadFileRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}