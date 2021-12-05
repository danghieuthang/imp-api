using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Features.Campaigns;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Influencers.Commands
{
    public class InviteInfluencerToCampaignCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }
        public int Id { get; set; }
        public class InviteInfluencerToCampaignCommandHandler : CommandHandler<InviteInfluencerToCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<ApplicationUser> _userRepository;
            private readonly INotificationService _notificationService;
            public InviteInfluencerToCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService, INotificationService notificationService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _userRepository = unitOfWork.Repository<ApplicationUser>();
                _notificationService = notificationService;
            }

            public override async Task<Response<bool>> Handle(InviteInfluencerToCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.InfluencerConfiguration, x => x.Brand);
                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Chiến dịch không tồn tại."));
                }

                if (campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Không có quyền."));
                }

                var influencer = await _userRepository.FindSingleAsync(x => x.Id == request.Id, x => x.InfluencerPlatforms);
                if (influencer == null)
                {
                    throw new ValidationException(new ValidationError("id", "Influencer không tồn tại."));
                }

                var cpM = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.CampaignId == campaign.Id && x.InfluencerId == request.Id);
                if (cpM != null)
                {
                    if (cpM.Status == (int)CampaignMemberStatus.Invited)
                        throw new ValidationException(new ValidationError("id", "Influencer này đã được mời."));
                    else
                    {
                        throw new ValidationException(new ValidationError("id", "Influencer này đang là thành viên trong chiến dịch."));
                    }
                }

                var errors = CampaignUtils.CheckSuitability(campaign, influencer);
                if (errors.Count > 0)
                {
                    throw new ValidationException(errors: errors);
                }

                CampaignMember member = new CampaignMember
                {
                    InfluencerId = request.Id,
                    CampaignId = campaign.Id,
                    Status = (int)CampaignMemberStatus.Invited,
                };


                UnitOfWork.Repository<CampaignMember>().Add(member);
                await UnitOfWork.CommitAsync();

                await _notificationService.PutNotication(applicationUserid: request.Id, redirectId: member.Id, notificationType: NotificationType.BrandInvitedToCampaign, $"Nhãn hàng - {campaign.Brand.CompanyName} vừa mời bạn tham gia chiến dịch.");
                return new Response<bool>(data: true);

            }
        }
    }
}
