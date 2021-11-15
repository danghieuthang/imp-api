using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.ApprovalCampaign
{
    public class ApprovalCampaignCommand : ICommand<CampaignViewModel>
    {
        public int Id { get; set; }
        public class ApprovalCampaignCommandHandler : CommandHandler<ApprovalCampaignCommand, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            public ApprovalCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
            }

            public override async Task<Response<CampaignViewModel>> Handle(ApprovalCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.GetByIdAsync(request.Id);
                if (campaign != null && campaign.Status == (int)CampaignStatus.Pending)
                {
                    campaign.Status = (int)CampaignStatus.Approved;
                    _campaignRepository.Update(campaign);
                    await UnitOfWork.CommitAsync();
                    var campaignViewModel = Mapper.Map<CampaignViewModel>(campaign);
                    return new Response<CampaignViewModel>(campaignViewModel);
                }
                throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));

            }
        }
    }
}
