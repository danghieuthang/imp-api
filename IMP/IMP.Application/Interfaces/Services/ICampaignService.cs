using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Services
{
    public interface ICampaignService
    {
        /// <summary>
        /// Scan campaign not closed. Then change their status if campaign milestone meet their time
        /// </summary>
        /// <returns></returns>
        Task AutoUpdateCampaignStatus();
        /// <summary>
        /// Get max total product amount of a campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        Task<decimal> BestCampaignMemberTotalProductAmount(int campaignId);

    }
}
