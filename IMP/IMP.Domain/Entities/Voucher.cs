using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Voucher : BaseEntity
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        [StringLength(256)]
        public string VoucherName { get; set; }
        [StringLength(256)]
        public string Image { get; set; }
        [StringLength(256)]
        public string Thumnail { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [Column(TypeName = "time(7)")]
        public TimeSpan? FromTime { get; set; }
        [Column(TypeName = "time(7)")]
        public TimeSpan? ToTime { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength(256)]
        public string Action { get; set; }
        [MaxLength(256)]
        public string Condition { get; set; }
        [MaxLength(256)]
        public string Target { get; set; }

        public ICollection<VoucherCode> VoucherCodes { get; set; }
    }
}
