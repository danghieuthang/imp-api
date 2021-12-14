using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.ProcessCampaignMember
{
    public class ProcessCampaignMemberCommand : ICommand<bool>
    {
        public int CampaignMemberId { get; set; }
        public CampaignMemberStatus Status { get; set; }
        public string Note { get; set; }

        public class ProcessCampaignMemberCommandHandler : CommandHandler<ProcessCampaignMemberCommand, bool>
        {
            private readonly IGenericRepository<CampaignMember> _campaignMemberRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly INotificationService _notificationService;

            public ProcessCampaignMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService, INotificationService notificationService) : base(unitOfWork, mapper)
            {
                _campaignMemberRepository = unitOfWork.Repository<CampaignMember>();
                _authenticatedUserService = authenticatedUserService;
                _notificationService = notificationService;
            }

            public override async Task<Response<bool>> Handle(ProcessCampaignMemberCommand request, CancellationToken cancellationToken)
            {
                var campaignMember = await _campaignMemberRepository.FindSingleAsync(x => x.Id == request.CampaignMemberId, x => x.Campaign);
                if (campaignMember == null)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không tồn tại"));
                }
                if (campaignMember.Campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new UnauthorizedAccessException();
                }

                if (campaignMember.Campaign.Status != (int)CampaignStatus.Applying)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Chiến dịch này đã qua thời gian duyệt."));
                }

                //if (campaignMember.Status != (int)CampaignMemberStatus.Pending)
                //{
                //    string requestStatus = campaignMember.Status == (int)CampaignMemberStatus.Approved ? "chấp nhận" : "từ chối";
                //    throw new ValidationException(new ValidationError("campaign_member_id", $"Yêu cầu này đã được {requestStatus}."));
                //}


                if (request.Status == CampaignMemberStatus.Approved)
                {
                    var campaignActivities = UnitOfWork.Repository<CampaignActivity>().GetAll(predicate: x => x.CampaignId == campaignMember.CampaignId).ToList();
                    var memberActivityRepository = UnitOfWork.Repository<MemberActivity>();
                    if (!await UnitOfWork.Repository<MemberActivity>().IsExistAsync(x => x.CampaignMemberId == campaignMember.Id))
                    {
                        string hashtags = campaignMember.Campaign.Hashtags ?? "[]";
                        var campaignHashtags = JsonConvert.DeserializeObject<List<string>>(hashtags);

                        SocialContent content = new SocialContent
                        {
                            Hashtags = campaignHashtags.Select(x => new HashtagChecker
                            {
                                Hashtag = x,
                                // Check hashtag is valid
                                IsValid = false,
                                IsUsedForAuthenticate = false,
                            }).ToList()
                        };
                        var page = await UnitOfWork.Repository<Page>().FindSingleAsync(x => x.InfluencerId == campaignMember.InfluencerId);
                        if (page != null)
                        {
                            content.Hashtags.Add(new HashtagChecker
                            {
                                Hashtag = "imp_" + page.BioLink,
                                IsUsedForAuthenticate = true,
                                IsValid = false,
                            });
                        }

                        foreach (var campaignActivity in campaignActivities)
                        {

                            memberActivityRepository.Add(new MemberActivity
                            {
                                CampaignActivityId = campaignActivity.Id,
                                CampaignMemberId = campaignMember.Id,
                                Status = (int)MemberActivityStatus.NotYet,
                                SocialContent = JsonConvert.SerializeObject(content)
                            });
                        }
                    }

                }
                else if (request.Status == CampaignMemberStatus.Cancelled)
                {
                    campaignMember.Note = request.Note;
                    var voucherCodeRepository = UnitOfWork.Repository<VoucherCode>();
                    var voucherCodes = voucherCodeRepository.GetAll(predicate: x => x.CampaignMemberId == campaignMember.Id).ToList();
                    foreach (var code in voucherCodes)
                    {
                        code.CampaignMemberId = null;
                        voucherCodeRepository.Update(code);
                    }
                }

                campaignMember.ApprovedDate = DateTime.Now;

                campaignMember.Status = (int)request.Status;
                campaignMember.ApprovedById = _authenticatedUserService.ApplicationUserId;
                _campaignMemberRepository.Update(campaignMember);
                await UnitOfWork.CommitAsync();

                if (campaignMember.Status == (int)CampaignMemberStatus.Cancelled)
                {
                    await _notificationService.PutNotication(campaignMember.InfluencerId, campaignMember.CampaignId, NotificationType.BrandCancelJoinCampaign);
                }
                else
                {
                    await _notificationService.PutNotication(campaignMember.InfluencerId, campaignMember.CampaignId, NotificationType.BrandApprovedJoinCampaign);
                }

                return new Response<bool>(true);
            }


            private Task SendNotification(int applicationUserId, int redirectId, CampaignMemberStatus status)
            {
                _ = Task.Run(() =>
                {
                    if (status == CampaignMemberStatus.Cancelled)
                    {
                        _notificationService.PutNotication(applicationUserId, redirectId, NotificationType.BrandCancelJoinCampaign);
                    }
                    else
                    {
                        _notificationService.PutNotication(applicationUserId, redirectId, NotificationType.BrandApprovedJoinCampaign);
                    }
                });
                return Task.CompletedTask;
            }

        }
    }
}
