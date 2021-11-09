using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.ApplyToCampaign
{
    public class ApplyToCampaignCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }

        public class ApplyToCampaignCommandHandler : CommandHandler<ApplyToCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<CampaignMember> _campaignMemberRepository;
            public ApplyToCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _campaignMemberRepository = unitOfWork.Repository<CampaignMember>();
            }

            public override async Task<Response<bool>> Handle(ApplyToCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.InfluencerConfiguration);
                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Chiến dịch không tồn tại."));
                }

                if (campaign.Status != (int)CampaignStatus.Applying)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Không thể thăm gia chiến dịch này."));
                }

                // Check Number of influencer of campaign
                int countMemberApplyToCampaign = await _campaignMemberRepository.CountAsync(x =>
                    x.CampaignId == request.CampaignId
                    && x.Status == (int)CampaignMemberStatus.Approved);

                if (campaign.InfluencerConfiguration.NumberOfInfluencer <= countMemberApplyToCampaign)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Chiến dịch đã đủ người."));
                }

                // Check Influencer is already apply to this campaign

                if (await _campaignMemberRepository.IsExistAsync(x => x.InfluencerId == _authenticatedUserService.ApplicationUserId && x.CampaignId == request.CampaignId))
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Đã có trong chiến dịch."));
                }

                CampaignMember member = new CampaignMember
                {
                    InfluencerId = _authenticatedUserService.ApplicationUserId,
                    CampaignId = campaign.Id,
                    Status = (int)CampaignMemberStatus.Pending
                };

                UnitOfWork.Repository<CampaignMember>().Add(member);
                await UnitOfWork.CommitAsync();

                return new Response<bool>(true);
            }
        }
    }
}
