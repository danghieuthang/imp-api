using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class VoucherCode : BaseEntity
    {
        [ForeignKey("Voucher")]
        public int VoucherId { get; set; }
        [MaxLength(256)]
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }

        [ForeignKey("CampaignMember")]
        public int? CampaignMemberId { get; set; }
        public DateTime? HoldTime { get; set; }


        public CampaignMember CampaignMember { get; set; }
        public Voucher Voucher { get; set; }


        public ICollection<VoucherTransaction> VoucherTransactions { get; set; }
        public ICollection<VoucherCodeApplicationUser> VoucherCodeApplicationUsers { get; set; }


    }
}
