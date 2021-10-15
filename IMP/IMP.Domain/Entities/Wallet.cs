using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public Wallet()
        {
            FromTransactions = new Collection<WalletTransaction>();
            ToTransactions = new Collection<WalletTransaction>();
        }
        public decimal Balance { get; set; }
        [ForeignKey("ApplicationUser")]
        public int? ApplicationUserId { get; set; }

        public ICollection<WalletTransaction> FromTransactions { get; set; }
        public ICollection<WalletTransaction> ToTransactions { get; set; }
    }
}
