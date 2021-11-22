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
        public DateTime ApprovedDate { get; set; }

        [ForeignKey("ApplicationUser")]
        public int? ApprovedById { get; set; }
        public ApplicationUser ApprovedBy { get; set; }

        public bool ActivityProgess { get; set; }
        public DateTime CompletedDate { get; set; }
        public decimal Money { get; set; }

        [StringLength(256)]
        public string Note { get; set; }

        public ICollection<ApplicantHistory> ApplicantHistories { get; set; }
        public ICollection<MemberActivity> MemberActivities { get; set; }
        public ICollection<VoucherCode> VoucherCodes { get; set; }
    }
}
