using IMP.Domain.Common;
using System;
using System.Collections;
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

        public int Status { get; set; }

        public ICollection<Evidence> Evidences { get; set; }

        public ICollection<ActivityComment> ActivityComments { get; set; }
    }
}
