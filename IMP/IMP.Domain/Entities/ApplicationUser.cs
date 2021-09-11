using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        [MaxLength(256)]
        public string UserName { get; set; }


        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        [ForeignKey("PaymentInfor")]
        public int PaymentInforId { get; set; }
        public PaymentInfor PaymentInfor { get; set; }

        [ForeignKey("Ranking")]
        public int? RankingId { get; set; }
        public Ranking Ranking { get; set; }

        public ICollection<Page> Pages { get; set; }
    }
}
