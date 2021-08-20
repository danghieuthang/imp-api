using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class VoucherTransaction : BaseEntity
    {
        [ForeignKey("VoucherCode")]
        public int VoucherCodeId { get; set; }

        public VoucherCode VoucherCode { get; set; }
    }
}
