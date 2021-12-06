using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class VoucherInfluencer : BaseEntity
    {
        [ForeignKey("Voucher")]
        public int VoucherId { get; set; }
        [ForeignKey("ApplicationUser")]
        public int InfluencerId { get; set; }

        public int QuantityGet { get; set; }
        public int QuantityUsed { get; set; }
        public ApplicationUser Influencer { get; set; }
        public Voucher Voucher { get; set; }
    }
}
