using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class PaymentInfor : BaseEntity
    {
        [ForeignKey("Bank")]
        public int? BankId { get; set; }
        public Bank Bank { get; set; }

        [StringLength(256)]
        public string AccountNumber { get; set; }
        [StringLength(256)]
        public string AccountName { get; set; }

        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
