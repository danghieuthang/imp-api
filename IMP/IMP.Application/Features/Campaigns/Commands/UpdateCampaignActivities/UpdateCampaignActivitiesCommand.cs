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

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaignActivities
{
    public class UpdateCampaignActivitiesCommand : ICommand<CampaignViewModel>
    {
        public int CampaignId { get; set; }
        public List<CampaignActivityUpdateModel> CampaignActivities { get; set; }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }
        public class UpdateCampaignActivitiesCommandHandler : CommandHandler<UpdateCampaignActivitiesCommand, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            private readonly IGenericRepository<CampaignActivity> _campaignActivityRepository;
            public UpdateCampaignActivitiesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
                _campaignActivityRepository = unitOfWork.Repository<CampaignActivity>();
            }

            public override async Task<Response<CampaignViewModel>> Handle(UpdateCampaignActivitiesCommand request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepository.FindSingleAsync(
                        predicate: campaigns => campaigns.Id == request.CampaignId,
                        include: x => x.Include(y => y.CampaignActivities));

                if (campaign != null)
                {
                if (campaign.Status != (int)CampaignStatus.Draft && campaign.Status != (int)CampaignStatus.Pending && campaign.Status != (int)CampaignStatus.Approved && campaign.Status != (int)CampaignStatus.Cancelled)

                    {
                        throw new ValidationException(new ValidationError("id", "Không thể chỉnh sửa chiến dịch này."));
                    }

                    // process activities
                    List<int> requestActivitiesIds = request.CampaignActivities.Select(x => x.Id).ToList();

                    foreach (var activity in campaign.CampaignActivities)
                    {
                        if (!requestActivitiesIds.Contains(activity.Id)) // delete activity not in request
                        {
                            _campaignActivityRepository.Delete(activity);
                        }
             
                    }

                    var campaignActivities = Mapper.Map<List<CampaignActivity>>(request.CampaignActivities);


                    campaign.CampaignActivities = campaignActivities.Select(x => { x.CampaignId = request.CampaignId; return x; }).ToList();

                    // If approved campaign then change status to pending
                    if (campaign.Status == (int)CampaignStatus.Approved)
                    {
                        campaign.Status = (int)CampaignStatus.Pending;
                    }

                    _campaignRepository.Update(campaign);
                    await UnitOfWork.CommitAsync();

                    var campaignView = Mapper.Map<CampaignViewModel>(campaign);
                    return new Response<CampaignViewModel>(campaignView);
                }
                throw new ValidationException(new ValidationError("campaign_id", "Không tồn tại."));
            }
        }
    }
}
