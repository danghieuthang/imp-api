using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMP.Domain.Common;

namespace IMP.Domain.Entities
{
    public class CampaignReward : BaseEntity
    {
        [StringLength(256)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [StringLength(50)]
        public string Currency { get; set; }

        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public bool IsDefaultReward { get; set; }
        public int RewardType { get; set; }
        [StringLength(2000)]
        public string IntervalReward { get; set; }
    }
}