using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class VoucherTransaction : BaseEntity
    {
        [ForeignKey("VoucherCode")]
        public int VoucherCodeId { get; set; }
        public int? CampaignId { get; set; }
        public decimal TotalProductAmount { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int ProductQuantity { get; set; }
        public string Order { get; set; }
        [StringLength(256)]
        public string Status { get; set; }
        public VoucherCode VoucherCode { get; set; }
    }
}
