﻿using AutoMapper;
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
                    throw new ValidationException(new ValidationError("campaign_id", "Chiến dịch không trong thời gian nhận influencer."));
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
                    throw new ValidationException(new ValidationError("campaign_id", "Đã thăm gia trong chiến dịch."));
                }

                // Check suitability
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == _authenticatedUserService.ApplicationUserId, x => x.InfluencerPlatforms);
                var errors = CheckSuitability(campaign, user);
                if (errors.Count > 0)
                {
                    return new Response<bool>(errors: errors, message: "Không phù hợp với chiến dịch.");
                }

                CampaignMember member = new CampaignMember
                {
                    InfluencerId = _authenticatedUserService.ApplicationUserId,
                    CampaignId = campaign.Id,
                    Status = (int)CampaignMemberStatus.Pending,
                };

                UnitOfWork.Repository<CampaignMember>().Add(member);
                await UnitOfWork.CommitAsync();

                return new Response<bool>(true);
            }

            private List<ValidationError> CheckSuitability(Campaign campaign, ApplicationUser user)
            {
                List<ValidationError> errors = new List<ValidationError>();

                // check marital
                if (campaign.InfluencerConfiguration.MaritalStatus.HasValue && campaign.InfluencerConfiguration.MaritalStatus != user.MaritalStatus)
                {
                    errors.Add(new ValidationError("", "Tình trạng hôn nhân không hợp lệ."));
                }

                // check child status
                if (campaign.InfluencerConfiguration.ChildStatus.HasValue && campaign.InfluencerConfiguration.ChildStatus != user.ChildStatus)
                {
                    errors.Add(new ValidationError("", "Tình trạng con cái không hợp lệ."));
                }

                // check pregnant
                //if (campaign.InfluencerConfiguration.Pregnant.HasValue && campaign.InfluencerConfiguration.Pregnant != user.Pregnant.Value)
                //{
                //    errors.Add(new ValidationError("", $"Tình trạng mang thai không hợp lệ."));
                //}

                if (campaign.InfluencerConfiguration.PlatformId.HasValue
                    && !user.InfluencerPlatforms.Select(x => x.PlatformId).Contains(campaign.InfluencerConfiguration.PlatformId.Value))
                {
                    errors.Add(new ValidationError("", $"Influencer chưa có hợp lệ."));
                }

                // check gender
                if (campaign.InfluencerConfiguration.Gender.HasValue)
                {
                    int gender;
                    switch (user.Gender.ToLower())
                    {
                        case "male":
                            gender = (int)Genders.Male;
                            break;
                        case "female":
                            gender = (int)Genders.Female;
                            break;
                        case "other":
                            gender = (int)Genders.Other;
                            break;
                        default:
                            gender = (int)Genders.None;
                            break;
                    }
                    if (gender != (int)Genders.None && gender != campaign.InfluencerConfiguration.Gender.Value)
                    {
                        errors.Add(new ValidationError("", $"Giới tính không hợp lệ."));
                    }
                }

                return errors;
            }
        }
    }
}
