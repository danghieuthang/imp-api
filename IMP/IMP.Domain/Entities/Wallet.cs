using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
