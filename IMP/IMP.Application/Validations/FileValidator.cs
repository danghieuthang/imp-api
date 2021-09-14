using FluentValidation;
using IMP.Application.Extensions;
using IMP.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Validations
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        private readonly FileSettings _fileSettings;
        public FileValidator(IOptions<FileSettings> options)
        {
            _fileSettings = options.Value;
            RuleFor(x => x).RequireFile();
            RuleFor(x => x.ContentType).ValidaFileType(_fileSettings.AllowTypes);
            RuleFor(x => x.Length).ValidFileSize(_fileSettings.MaximumSize);
        }
    }
}
