using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class CampaignMember : BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        [ForeignKey("ApplicationUser")]
        public int InfluencerId { get; set; }
        public ApplicationUser Influencer { get; set; }
        public int Status { get; set; }

        public ICollection<ApplicantHistory> ApplicantHistories { get; set; }
        public ICollection<MemberActivity> MemberActivities { get; set; }
    }
}
