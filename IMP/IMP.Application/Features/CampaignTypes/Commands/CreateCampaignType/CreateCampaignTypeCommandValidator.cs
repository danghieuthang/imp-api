using System.Threading;
using System;
using AutoMapper;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Validations;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType
{
    public class CreateCampaignTypeCommandValidator : AbstractValidator<CreateCampaignTypeCommand>
    {
        private readonly ICampaignTypeRepositoryAsync _campaignTypeRepostoryAsync;

        public CreateCampaignTypeCommandValidator(ICampaignTypeRepositoryAsync campaignTypeRepositoryAsync, IOptions<FileSettings> options)
        {
            _campaignTypeRepostoryAsync = campaignTypeRepositoryAsync;
            RuleFor(x => x.Name).Required(256)
                .MustAsync(IsUniqueCampaignType).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).Required(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));

            RuleFor(x => x.ParentId).MustAsync(IsExistCampaignType).WithMessage("'{PropertyValue}' không tồn tại.");
        }
        private async Task<bool> IsUniqueCampaignType(string name, CancellationToken cancellationToken)
        {
            return await _campaignTypeRepostoryAsync.IsUniqueCampaignType(name);
        }

        private async Task<bool> IsExistCampaignType(int? id, CancellationToken cancellationToken)
        {
            if (id == null) return true;
            return await _campaignTypeRepostoryAsync.IsExistAsync(id.Value);
        }
    }
}