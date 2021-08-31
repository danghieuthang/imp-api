using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class MemberActivity : BaseEntity
    {
        [ForeignKey("CampaignActivity")]
        public int CampaignActivityId { get; set; }
        public CampaignActivity CampaignActivity { get; set; }

        [ForeignKey("CampaignMember")]
        public int CampaignMemberId { get; set; }
        public CampaignMember CampaignMember { get; set; }

        [ForeignKey("ActivityType")]
        public int ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }

        public ApplicationUser Influencer { get; set; }
        [Range(0, 100)]
        public float Progress { get; set; }
        public int Position { get; set; }

        public ICollection<Evidence> Evidences { get; set; }
    }
}
