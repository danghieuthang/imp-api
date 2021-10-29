using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Campaign> _campaignRepostory;
        public CampaignService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _campaignRepostory = _unitOfWork.Repository<Campaign>();
        }

        public async Task AutoUpdateCampaignStatus()
        {
            var campaigns = await _campaignRepostory.GetAll(
                predicate: x => x.Status != (int)CampaignStatus.Draft
                            && x.Status != (int)CampaignStatus.Closed
                            && x.Status != (int)CampaignStatus.Pending
                            && x.Status != (int)CampaignStatus.Canceled,
                orderBy: x => x.OrderBy(y => y.Created)).ToListAsync();

            foreach (var campaign in campaigns)
            {
                // get Indochina time
                DateTime now = DateTime.UtcNow.AddHours(7);
                if (now >= campaign.Openning && now < campaign.Applying)
                {
                    campaign.Status = (int)CampaignStatus.Openning;
                }
                else if (now >= campaign.Applying && now < campaign.Advertising)
                {
                    campaign.Status = (int)CampaignStatus.Applying;
                }
                else if (now >= campaign.Advertising && now < campaign.Evaluating)
                {
                    campaign.Status = (int)CampaignStatus.Advertising;
                }
                else if (now >= campaign.Evaluating && now < campaign.Announcing)
                {
                    campaign.Status = (int)CampaignStatus.Evaluating;
                }
                else if (now >= campaign.Announcing && now < campaign.Closed)
                {
                    campaign.Status = (int)CampaignStatus.Announcing;
                }
                else if (now >= campaign.Closed)
                {
                    campaign.Status = (int)CampaignStatus.Closed;
                }
                _campaignRepostory.Update(campaign);

            }
            await _unitOfWork.CommitAsync();

        }
    }
}

