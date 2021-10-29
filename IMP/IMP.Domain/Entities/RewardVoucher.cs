using System.ComponentModel.DataAnnotations.Schema;
using IMP.Domain.Common;

namespace IMP.Domain.Entities
{
    public class RewardVoucher : BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        [ForeignKey("CampaignVoucher")]
        public int CampaignVoucherId { get; set; }

        public CampaignVoucher CampaignVoucher { get; set; }

        public bool IsFixedReward { get; set; }
    }
}