using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Repositories;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommand>
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IGenericRepository<Platform> _platformRepository;
        private readonly IGenericRepository<CampaignType> _campaignTypeRepository;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;
        public CreateCampaignCommandValidator(IUnitOfWork unitOfWork)
        {
            _campaignRepository = unitOfWork.Repository<Campaign>();
            _platformRepository = unitOfWork.Repository<Platform>();
            _campaignTypeRepository = unitOfWork.Repository<CampaignType>();
            _applicationUserRepository = unitOfWork.Repository<ApplicationUser>();

            this.RuleFor(c => c.Title).MustRequired(256);
            this.RuleFor(c => c.AdditionalInfomation).MustRequired(2000);
            this.RuleFor(c => c.Description).MustRequired(2000);
            this.RuleFor(c => c.Condition).MustRequired(2000);
            this.RuleFor(c => c.StartDate).MustValidDate();
            this.RuleFor(c => c.EndDate).MustValidDate()
                .Must((command, date) =>
               {
                   if (!command.StartDate.HasValue || !command.EndDate.HasValue) return true;
                   return command.EndDate.Value.CompareTo(command.StartDate) > 0;
               }).WithMessage("{PropertyValue} lớn hơn ngày bắt đầu chiến dịch.");

            this.RuleFor(c => c.PlatformId).MustExistEntityId(IsPlatformExist);
            this.RuleFor(c => c.CampaignTypeId).MustExistEntityId(IsCampainTypeExist);
            this.RuleFor(c => c.BrandId).MustAsync(IsValidBrand).WithMessage("{PropertyValue} không phải là nhãn hàng.");
            this.RuleFor(c => c.Images).ListMustContainFewerThan(5);
            this.RuleForEach(c => c.Images).ChildRules(image =>
            {
                image.RuleFor(image => image.Url).MustValidUrl();
            }).When(c => c.Images.Count > 0);
        }

        public async Task<bool> IsPlatformExist(int id, CancellationToken cancellationToken)
        {
            return await _platformRepository.IsExistAsync(id);
        }

        public async Task<bool> IsCampainTypeExist(int id, CancellationToken cancellationToken)
        {
            return await _campaignTypeRepository.IsExistAsync(id);
        }

        public async Task<bool> IsValidBrand(int id, CancellationToken cancellationToken)
        {
            var user = await _applicationUserRepository.GetByIdAsync(id);
            return user != null;
        }
    }
}
