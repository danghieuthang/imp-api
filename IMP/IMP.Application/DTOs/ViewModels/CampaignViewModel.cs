using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.DTOs.Compaign
{
    public class CampaignViewModel : BaseViewModel<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string AditionalInfomation { get; set; }
        public string Reward { get; set; }
        public string ReferalWebsite { get; set; }
        public string Keywords { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxInfluencer { get; set; }
        public int Status { get; set; }
        public string Condition { get; set; }
        public bool IsActived { get; set; }
        public DateTime New { get; set; }
        public DateTime Applying { get; set; }
        public DateTime Selecting { get; set; }
        public DateTime Posting { get; set; }
        public DateTime Announced { get; set; }
        public DateTime Closing { get; set; }
    }
}
