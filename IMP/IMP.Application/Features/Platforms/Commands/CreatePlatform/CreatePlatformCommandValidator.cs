using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Validations;
using IMP.Domain.Entities;
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
        private readonly IGenericRepository<Platform> _platformRepositoryAsync;
        public CreatePlatformCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _platformRepositoryAsync = unitOfWork.Repository<Platform>();
            this.RuleFor(x => x.Name).MustRequired(256)
                .MustAsync(IsUniquePlatform).WithMessage($"Tên đã tồn tại.");

            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        private async Task<bool> IsUniquePlatform(string name, CancellationToken cancellationToken)
        {
            var entity = await _platformRepositoryAsync.FindSingleAsync(x => x.Name.ToLower() == name.ToLower());
            return entity == null;
        }

    }
}
