using System;

namespace IMP.Application.Models.Compaign
{
    public class CampaignMilestoneViewModel : BaseViewModel<int>
    {
        public int MilestoneId { get; set; }
        //public MilestoneViewModel Milestone { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}