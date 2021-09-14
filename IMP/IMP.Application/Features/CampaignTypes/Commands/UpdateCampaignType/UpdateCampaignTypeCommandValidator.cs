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

namespace IMP.Application.Features.CampaignTypes.Commands.UpdateCampaignType
{
    public class UpdateCampaignTypeCommandValidator : AbstractValidator<UpdateCampaignTypeCommand>
    {
        private readonly IGenericRepositoryAsync<CampaignType> _campaignTypeRepostoryAsync;

        public UpdateCampaignTypeCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> options)
        {
            _campaignTypeRepostoryAsync = unitOfWork.Repository<CampaignType>();

            RuleFor(x => x.Id).MustAsync(IsExistCampaignType).WithMessage("{PropertyValue} không tồn tại.");
            RuleFor(x => x.Name).Required(256)
               .MustAsync((campaignType, name, cancellationToken) =>
               {
                   return IsUniqueCampaignType(campaignType.Id, name, cancellationToken);
               }).WithMessage("'{PropertyValue}' đã tồn tại.");

            RuleFor(x => x.Description).Required(2000);
            RuleFor(x => x.ImageFile).SetValidator(new FileValidator(options));
            RuleFor(x => x.ParentId).MustAsync(IsExistCampaignType).WithMessage("'{PropertyValue}' không tồn tại.");
        }

        private async Task<bool> IsUniqueCampaignType(int id, string name, CancellationToken cancellationToken)
        {
            var entity = await _campaignTypeRepostoryAsync.FindSingleAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());
            return entity == null;
        }

        private async Task<bool> IsExistCampaignType(int? id, CancellationToken cancellationToken)
        {
            if (id == null) return true;
            return await _campaignTypeRepostoryAsync.IsExistAsync(id.Value);
        }

        private async Task<bool> IsExistCampaignType(int id, CancellationToken cancellationToken)
        {
            return await _campaignTypeRepostoryAsync.IsExistAsync(id);
        }

    }
}
