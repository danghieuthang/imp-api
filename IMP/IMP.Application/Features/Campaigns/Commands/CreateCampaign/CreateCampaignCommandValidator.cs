using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Repositories;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommand>
    {
        private readonly IGenericRepositoryAsync<Campaign> _campaignRepositoryAsync;
        private readonly IGenericRepositoryAsync<Platform> _platformRepositoryAsync;
        private readonly IGenericRepositoryAsync<CampaignType> _campaignTypeRepositoryAsync;
        private readonly IGenericRepositoryAsync<ApplicationUser> _applicationUserRepositoryAsync;
        public CreateCampaignCommandValidator(IUnitOfWork unitOfWork)
        {
            _campaignRepositoryAsync = unitOfWork.Repository<Campaign>();
            _platformRepositoryAsync = unitOfWork.Repository<Platform>();
            _campaignTypeRepositoryAsync = unitOfWork.Repository<CampaignType>();
            _applicationUserRepositoryAsync = unitOfWork.Repository<ApplicationUser>();

            this.RuleFor(c => c.Title).Required(256);
            this.RuleFor(c => c.AditionalInfomation).Required(2000);
            this.RuleFor(c => c.Description).Required(2000);
            this.RuleFor(c => c.Condition).Required(2000);
            this.RuleFor(c => c.StartDate).IsValidDate();
            this.RuleFor(c => c.EndDate).IsValidDate()
                .MustAsync(async (command, date, cancelationToken) =>
                {
                    if (!command.StartDate.HasValue || !command.EndDate.HasValue) return true;
                    return command.EndDate.Value.CompareTo(command.StartDate) > 0;
                }).WithMessage("{PropertyValue} lớn hơn ngày bắt đầu chiến dịch.");

            this.RuleFor(c => c.PlatformId).IsExistId(IsPlatformExist);
            this.RuleFor(c => c.CampaignTypeId).IsExistId(IsCampainTypeExist);
            this.RuleFor(c => c.BrandId).MustAsync(IsValidBrand).WithMessage("{PropertyValue} không phải là nhãn hàng.");
        }

        public async Task<bool> IsPlatformExist(int id, CancellationToken cancellationToken)
        {
            return await _platformRepositoryAsync.IsExistAsync(id);
        }

        public async Task<bool> IsCampainTypeExist(int id, CancellationToken cancellationToken)
        {
            return await _campaignTypeRepositoryAsync.IsExistAsync(id);
        }

        public async Task<bool> IsValidBrand(int id, CancellationToken cancellationToken)
        {
            var user = await _applicationUserRepositoryAsync.GetByIdAsync(id);
            return user != null;
        }
    }
}
