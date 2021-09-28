using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.Compaign
{
    public class CampaignViewModel : BaseViewModel<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AdditionalInfomation { get; set; }
        public string Reward { get; set; }
        public string ReferralWebsite { get; set; }
        public string Keywords { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxInfluencer { get; set; }
        public int Status { get; set; }
        public string Condition { get; set; }
        public bool IsActived { get; set; }
        public List<CampaignImageViewModel> CampaignImages { get; set; }
    }

    public class CampaignImageViewModel : BaseViewModel<int>
    {
        public int Position { get; set; }
        public string Url { get; set; }
    }
}
