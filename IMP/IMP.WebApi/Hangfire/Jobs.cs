using Hangfire;
using IMP.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace IMP.WebApi.Hangfire
{
    public class BackgroundJobs
    {
        private readonly ICampaignService _campaignService;

        public BackgroundJobs(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<bool> CampaignJobsAsync()
        {
            await _campaignService.AutoUpdateCampaignStatus();
            return true;
        }

        //public async Task<bool>
    }
}
