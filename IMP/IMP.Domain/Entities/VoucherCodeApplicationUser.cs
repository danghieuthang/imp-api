using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Entities
{
    public class VoucherCodeApplicationUser : BaseEntity
    {
        [ForeignKey("VoucherCode")]
        public int VoucherCodeId { get; set; }
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public VoucherCode VoucherCode { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
