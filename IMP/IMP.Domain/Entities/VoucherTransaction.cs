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
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Tổng tiền được hưởng giảm giá
        /// </summary>
        public decimal TotalDiscount { get; set; }
        public string Order { get; set; }
        [StringLength(256)]
        public string OrderCode { get; set; }
        public DateTime OrderCreated { get; set; }
        public VoucherCode VoucherCode { get; set; }
    }
}
