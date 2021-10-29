using FluentValidation;
using IMP.Application.Enums;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaignInfluencerConfiguration
{
    public class UpdateInfluencerConfigurationCommandValidator : AbstractValidator<UpdateInfluencerConfigurationCommand>
    {
        public UpdateInfluencerConfigurationCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AgeFrom).Must((x, y) =>
            {
                if (!x.AgeFrom.HasValue || !x.AgeTo.HasValue)
                {
                    return true;
                }
                return x.AgeTo >= x.AgeFrom;
            }
            ).WithMessage("Tuổi không hợp lệ");

            RuleFor(x => x.Gender).IsEnumName(typeof(Genders), caseSensitive: false).WithMessage("Giới tính không có {PropertyValue}");

            RuleFor(x => x.CampaignId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<Campaign>().IsExistAsync(x);
            }).WithMessage("Chiến dịch không tồn tại.");

            List<int> locationIds = unitOfWork.Repository<Location>().GetAll().Select(x => x.Id).ToList();

            RuleForEach(x => x.Locations).ChildRules(locations =>
            {
                locations.RuleFor(x => x.LocationId).Must(locationId => locationIds.Contains(locationId)).WithMessage("Khu vực không tồn tại.");
            });

            RuleFor(x => x.LevelId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<RankLevel>().IsExistAsync(x);
            }).WithMessage("Không hợp lệ.");

            RuleFor(x => x.PlatformId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<Platform>().IsExistAsync(x);
            }).WithMessage("Không hợp lệ.");
        }
    }
}
