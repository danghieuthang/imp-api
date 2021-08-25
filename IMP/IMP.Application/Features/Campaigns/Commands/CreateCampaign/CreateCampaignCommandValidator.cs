using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommandValidator : AbstractValidator<CreateCampaignCommand>
    {
        private readonly ICampaignRepositoryAsync _campaignRepositoryAsync;
        public CreateCampaignCommandValidator(ICampaignRepositoryAsync campaignRepositoryAsync)
        {
            _campaignRepositoryAsync = campaignRepositoryAsync;

            this.RuleFor(c => c.Title).Required(256);
            this.RuleFor(c => c.AditionalInfomation).Required(2000);
            this.RuleFor(c => c.Description).Required(2000);
            this.RuleFor(c => c.Condition).Required(2000);
        }
    }
}
