using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class WalletTransaction: BaseEntity
    {
        [ForeignKey("Wallet")]
        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }
    }
}
