using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaignActivities
{
    public class UpdateCampaignActivitiesCommandValidator : AbstractValidator<UpdateCampaignActivitiesCommand>
    {
        public UpdateCampaignActivitiesCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.CampaignId).MustExistEntityId(async (id, c) =>
              {
                  return await unitOfWork.Repository<Campaign>().IsExistAsync(id);
              });

            RuleForEach(x => x.CampaignActivities).SetValidator(new CampaignActivityValidator(unitOfWork));
        }

    }
    public class CampaignActivityValidator : AbstractValidator<CampaignActivityUpdateModel>
    {
        public CampaignActivityValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Name).MustMaxLength(256);
            RuleFor(x => x.Description).MustMaxLength(2000);
            RuleFor(x => x.HowToDo).MustMaxLength(2000);
            RuleFor(x => x.ActivityTypeId).MustExistEntityId(async (id, c) =>
            {
                return await unitOfWork.Repository<ActivityType>().IsExistAsync(id);
            });

            RuleForEach(x => x.ActivityResults).ChildRules(results =>
            {
                results.RuleFor(x => x.Name).MustMaxLength(256);
            });
        }
    }
}
