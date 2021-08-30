﻿using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
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

namespace IMP.Application.Features.Platforms.Commands.UpdatePlatform
{
    public class UpdatePlatformCommandValidation : AbstractValidator<UpdatePlatformCommand>
    {
        private readonly IPlatformRepositoryAsync _platformRepositoryAsync;

        public UpdatePlatformCommandValidation(IPlatformRepositoryAsync platformRepositoryAsync, IOptions<FileSettings> options)
        {
            _platformRepositoryAsync = platformRepositoryAsync;

            RuleFor(x => x.Id).NotNull().WithMessage("Chưa có.")
                .MustAsync(IsExist).WithMessage("'{PropertyValue}' không tồn tại.");

            RuleFor(x => x.Name).Required(256)
                .MustAsync(
                async (o, name, cancellationToken) =>
                {
                    return await IsUniquePlatformUpdate(name, o.Id, cancellationToken);
                }).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
        }

        private async Task<bool> IsUniquePlatformUpdate(string name, int id, CancellationToken cancellationToken)
        {
            return await _platformRepositoryAsync.IsUniquePlatform(name, id);
        }
        private async Task<bool> IsExist(int id, CancellationToken cancellationToken)
        {
            return await _platformRepositoryAsync.IsExistAsync(id);
        }
    }


}

