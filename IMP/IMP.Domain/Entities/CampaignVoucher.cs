using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class CampaignVoucher : BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        [ForeignKey("Voucher")]
        public int VoucherId { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int PercentForInfluencer { get; set; }
        public int PercentForIMP { get; set; }

        public ICollection<RewardVoucher> RewardVouchers { get; set; }
    }
}
