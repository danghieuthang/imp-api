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
    public class DraftCampagnPendingCommand : ICommand<CampaignViewModel>
    {
        public int Id { get; set; }
        public class DraftCampaignPendingCommandHandler : CommandHandler<DraftCampagnPendingCommand, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public DraftCampaignPendingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _campaignRepository = unitOfWork.Repository<Campaign>();

            }

            public override async Task<Response<CampaignViewModel>> Handle(DraftCampagnPendingCommand request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.GetByIdAsync(request.Id);
                if (campaign == null)
                {
                    throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));
                }
                if (campaign.Status != (int)CampaignStatus.Pending)
                {
                    throw new ValidationException(new ValidationError("id", "Chỉ có thể chuyển chiến từ trạng thái 'chờ duyệt' sang 'rác'."));
                }
                if (campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("id", "Không có quyền."));

                }
                campaign.Status = (int)CampaignStatus.Draft;

                _campaignRepository.Update(campaign);
                await UnitOfWork.CommitAsync();
                var campaignViewModel = Mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(campaignViewModel);
            }
        }
    }
}
