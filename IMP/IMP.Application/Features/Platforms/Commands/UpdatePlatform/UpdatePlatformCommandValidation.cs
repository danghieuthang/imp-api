using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
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

namespace IMP.Application.Features.Platforms.Commands.UpdatePlatform
{
    public class UpdatePlatformCommandValidation : AbstractValidator<UpdatePlatformCommand>
    {
        private readonly IGenericRepository<Platform> _platformRepositoryAsync;

        public UpdatePlatformCommandValidation(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _platformRepositoryAsync = unitOfWork.Repository<Platform>();

            RuleFor(x => x.Id).NotNull().WithMessage("Chưa có.")
                .MustAsync(IsExist).WithMessage("'{PropertyValue}' không tồn tại.");

            RuleFor(x => x.Name).MustRequired(256)
                .MustAsync(
                async (o, name, cancellationToken) =>
                {
                    return await IsUniquePlatformUpdate(name, o.Id, cancellationToken);
                }).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        private async Task<bool> IsUniquePlatformUpdate(string name, int id, CancellationToken cancellationToken)
        {
            var entity = await _platformRepositoryAsync.FindSingleAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());
            return entity == null;
        }
        private async Task<bool> IsExist(int id, CancellationToken cancellationToken)
        {
            return await _platformRepositoryAsync.IsExistAsync(id);
        }
    }


}

