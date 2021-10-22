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
            //var campaigns = await _campaignRepostory.GetAll(
            //    predicate: x => x.Status != (int)CampaignStatus.DRAFT && x.Status != (int)CampaignStatus.CLOSED,
            //    orderBy: x => x.OrderBy(y => y.Created),
            //    include: x => x.Include(y => y.CampaignMilestones)).ToListAsync();

            //foreach (var campaign in campaigns)
            //{
            //    foreach (var campaignMilestone in campaign.CampaignMilestones)
            //    {
            //        // get Indochina time
            //        DateTime now = DateTime.UtcNow.AddHours(7);
            //        if (campaignMilestone.FromDate <= now && campaignMilestone.ToDate >= now)
            //        {
            //            switch (campaignMilestone.MilestoneId)
            //            {
            //                // if nộp đơn và duyệt đdơn
            //                case 1:
            //                case 2:
            //                    campaign.Status = (int)CampaignStatus.APPROVED;
            //                    break;
            //                // if quảng cáo
            //                case 3:
            //                    campaign.Status = (int)CampaignStatus.PROCESSING;
            //                    break;
            //                // if thông báo
            //                case 4:
            //                    campaign.Status = (int)CampaignStatus.ANNOUNCED;
            //                    break;
            //            }
            //        }
            //    }
            //    _campaignRepostory.Update(campaign);
            //}
            //await _unitOfWork.CommitAsync();
        }
    }
}
