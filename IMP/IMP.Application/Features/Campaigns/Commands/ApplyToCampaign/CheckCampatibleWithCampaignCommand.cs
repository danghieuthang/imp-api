using AutoMapper;
using IMP.Application.Constants;
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
    public class CheckCampatibleWithCampaignCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }
        public bool IsCheckSuitable { get; set; }
        public class CheckCampatibleWithCampaignCommandHandler : CommandHandler<CheckCampatibleWithCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<CampaignMember> _campaignMemberRepository;
            public CheckCampatibleWithCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _campaignMemberRepository = unitOfWork.Repository<CampaignMember>();
            }

            public override async Task<Response<bool>> Handle(CheckCampatibleWithCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.InfluencerConfiguration);
                if (campaign == null)
                {
                    return new Response<bool>(new ValidationError("campaign_id", "Chiến dịch không tồn tại."), code: ErrorConstants.Application.Campaign.NotFound);
                }

                if (request.IsCheckSuitable)
                {
                    // Check suitability
                    var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == _authenticatedUserService.ApplicationUserId, x => x.InfluencerPlatforms);
                    var errors = CampaignUtils.CheckSuitability(campaign, user);
                    if (errors.Count > 0)
                    {
                        return new Response<bool>(errors: errors, message: "Không phù hợp với chiến dịch.", code: ErrorConstants.Application.Campaign.NotSuitable);
                    }
                }
                else
                {
                    // Check Influencer is already apply to this campaign

                    if (await _campaignMemberRepository.IsExistAsync(x => x.InfluencerId == _authenticatedUserService.ApplicationUserId && x.CampaignId == request.CampaignId))
                    {
                        return new Response<bool>(message:"Đã có trong chiến dịch.", code: ErrorConstants.Application.Campaign.AlreadyJoined);
                    }
                }

                return new Response<bool>(data: true, message: "Có thể thăm gia chiến dịch.");
            }
        }
    }
}
