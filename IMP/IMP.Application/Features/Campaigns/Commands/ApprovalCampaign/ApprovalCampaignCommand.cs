using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
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
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly INotificationService _notificationService;
            public ApprovalCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService, INotificationService notificationService) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
                _authenticatedUserService = authenticatedUserService;
                _notificationService = notificationService;
            }

            public override async Task<Response<CampaignViewModel>> Handle(ApprovalCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.GetByIdAsync(request.Id);
                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));
                }
                if (campaign.Status != (int)CampaignStatus.Pending && campaign.Status != (int)CampaignStatus.Cancelled)
                {
                    throw new ValidationException(new ValidationError("id", "Không thể duyệt chiến dịch này."));
                }

                if (campaign.ApplyingDate >= DateTime.UtcNow)
                {
                    campaign.Status = (int)CampaignStatus.Applying;
                }
                else
                {
                    campaign.Status = (int)CampaignStatus.Approved;
                }
                campaign.ApprovedById = _authenticatedUserService.ApplicationUserId;
                _campaignRepository.Update(campaign);
                await UnitOfWork.CommitAsync();

                await _notificationService.PutNotication(campaign.CreatedById, campaign.Id, NotificationType.AdminApprovedCampaign);

                var campaignViewModel = Mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(campaignViewModel);
            }

        }
    }
}
