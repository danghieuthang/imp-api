using Hangfire;
using IMP.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace IMP.WebApi.Hangfire
{
    public class BackgroundJobs
    {
        private readonly ICampaignService _campaignService;
        private readonly IMemberActivityService _memberActivityService;

        public BackgroundJobs(ICampaignService campaignService, IMemberActivityService memberActivityService)
        {
            _campaignService = campaignService;
            _memberActivityService = memberActivityService;
        }

        public async Task<bool> CampaignJobsAsync()
        {
            await _campaignService.AutoUpdateCampaignStatus();
            return true;
        }

        public async Task<bool> AnalysisFacebookPost()
        {
            await _memberActivityService.AutoCheckActivityInSocial();
            return true;
        }

        //public async Task<bool>
    }
}
