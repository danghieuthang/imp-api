using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        [StringLength(256)]
        public string TransactionInfo { get; set; }
        [StringLength(256)]
        public string Note { get; set; }
        [ForeignKey("Bank")]
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
        /// <summary>
        /// Number transaction at banking
        /// </summary>
        [StringLength(256)]
        public string BankTranNo { get; set; }
        /// <summary>
        /// Number transaction at vnpay
        /// </summary>
        [StringLength(256)]
        public string VnpTransactionNo { get; set; }
        /// <summary>
        /// Payment time, recorded at VNPAY in GMT+7
        /// </summary>
        public DateTime PayDate { get; set; }
        public int TransactionStatus { get; set; }
        public int TransactionType { get; set; }
        public decimal? SenderBalance { get; set; }
        public decimal? ReceiverBalance { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }

        public int? WalletToId { get; set; }
        public int? WalletFromId { get; set; }
        public string Evidences { get; set; }

        public virtual Wallet WalletTo { get; set; }
        public virtual Wallet WalletFrom { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}
