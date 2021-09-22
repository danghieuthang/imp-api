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
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType
{
    public class CreateCampaignTypeCommandValidator : AbstractValidator<CreateCampaignTypeCommand>
    {
        private readonly IGenericRepository<CampaignType> _campaignTypeRepostoryAsync;

        public CreateCampaignTypeCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _campaignTypeRepostoryAsync = unitOfWork.Repository<CampaignType>();
            RuleFor(x => x.Name).MustRequired(256)
                .MustAsync(IsUniqueCampaignType).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).MustRequired(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));

            RuleFor(x => x.ParentId).MustAsync(IsExistCampaignType).WithMessage("'{PropertyValue}' không tồn tại.");
        }
        private async Task<bool> IsUniqueCampaignType(string name, CancellationToken cancellationToken)
        {
            var entity = await _campaignTypeRepostoryAsync.FindSingleAsync(x => x.Name.ToLower() == name.ToLower());
            return entity == null;
        }

        private async Task<bool> IsExistCampaignType(int? id, CancellationToken cancellationToken)
        {
            if (id == null) return true;
            return await _campaignTypeRepostoryAsync.IsExistAsync(id.Value);
        }
    }
}