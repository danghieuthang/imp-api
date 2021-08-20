using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class ApplicantHistory: BaseEntity
    {
        [ForeignKey("CampaignMember")]
        public int CampaignMemberId { get; set; }
        public CampaignMember CampaignMember { get; set; }

        [StringLength(256)]
        public string Note { get; set; }
        [StringLength(256)]
        public string PreChanged { get; set; }
        [StringLength(256)]
        public string CurrentChanged { get; set; }
    }
}
