using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Campaign : BaseEntity
    {
        [ForeignKey("Platform")]
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        [ForeignKey("CampaignType")]
        public int CampaignTypeId { get; set; }
        public CampaignType CampaignType { get; set; }

        [ForeignKey("ApplicationUser")]
        public int BrandId { get; set; }
        public ApplicationUser Brand { get; set; }

        [StringLength(256)]
        public string Title { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [StringLength(2000)]
        public string AdditionalInformation { get; set; }
        [StringLength(2000)]
        public string ProductInformation { get; set; }
        [StringLength(2000)]
        public string Reward { get; set; }
        [StringLength(256)]
        public string ReferralWebsite { get; set; }
        public string Keywords { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxInfluencer { get; set; }
        public int Status { get; set; }
        [StringLength(2000)]
        public string Condition { get; set; }
        public bool IsActived { get; set; }

        public ICollection<CampaignMember> CampaignMembers { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }
        public ICollection<CampaignActivity> CampaignActivities { get; set; }
        public ICollection<BlockCampaign> BlockCampaigns { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<CampaignMilestone> CampaignMilestones { get; set; }
        public ICollection<CampaignImage> CampaignImages { get; set; }
    }
}
