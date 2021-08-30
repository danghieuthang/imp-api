using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Validations;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Platforms.Commands.CreatePlatform
{
    public class CreatePlatformCommandValidator : AbstractValidator<CreatePlatformCommand>
    {
        private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
        public CreatePlatformCommandValidator(IPlatformRepositoryAsync platformRepositoryAsync, IOptions<FileSettings> options)
        {
            _platformRepositoryAsync = platformRepositoryAsync;
            this.RuleFor(x => x.Name).Required(256)
                .MustAsync(IsUniquePlatform).WithMessage($"Tên đã tồn tại.");

            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));

        }

        private async Task<bool> IsUniquePlatform(string name, CancellationToken cancellationToken)
        {
            return await _platformRepositoryAsync.IsUniquePlatform(name);
        }

    }
}
