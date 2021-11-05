using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.CompletedCreateCampaign
{
    public class CompletedCreateCampaignCommand : ICommand<CampaignViewModel>
    {
        public int Id { get; set; }

        public class CompletedCreateCampaignCommandHandler : CommandHandler<CompletedCreateCampaignCommand, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            public CompletedCreateCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
            }

            public override async Task<Response<CampaignViewModel>> Handle(CompletedCreateCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.FindSingleAsync(predicate: x => x.Id == request.Id);

                if (campaign != null && campaign.Status == (int)CampaignStatus.Draft)
                {
                    campaign.Status = (int)CampaignStatus.Pending;

                    _campaignRepository.Update(campaign);
                    await UnitOfWork.CommitAsync();

                    var campaignView = Mapper.Map<CampaignViewModel>(campaign);
                    return new Response<CampaignViewModel>(campaignView);
                }

                throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));
            }
        }
    }
}
