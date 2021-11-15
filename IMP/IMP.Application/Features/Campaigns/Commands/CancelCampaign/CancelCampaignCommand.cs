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

namespace IMP.Application.Features.Campaigns.Commands.CancelCampaign
{
    public class CancelCampaignCommand : ICommand<CampaignViewModel>
    {
        public int Id { get; set; }
        public string Note { get; set; }

        public class CancelCampaignCommandHandler : CommandHandler<CancelCampaignCommand, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CancelCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CampaignViewModel>> Handle(CancelCampaignCommand request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.GetByIdAsync(request.Id);
                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));
                }
                if (campaign.Status != (int)CampaignStatus.Pending && campaign.Status != (int)CampaignStatus.Approved)
                {
                    throw new ValidationException(new ValidationError("id", "Chỉ có thể đóng chiến dịch đang 'chờ duyệt' hoặc 'đã duyệt'."));
                }

                campaign.Status = (int)CampaignStatus.Cancelled;
                campaign.Note = request.Note;
                campaign.ApprovedById = _authenticatedUserService.ApplicationUserId;

                _campaignRepository.Update(campaign);
                await UnitOfWork.CommitAsync();
                var campaignViewModel = Mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(campaignViewModel);
            }
        }
    }
}
