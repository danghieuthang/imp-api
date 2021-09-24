﻿using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public string TransactionInfo { get; set; }
        [ForeignKey("Bank")]
        public int BankId { get; set; }
        public Bank Bank { get; set; }
        /// <summary>
        /// Number transaction at banking
        /// </summary>
        public int BankTranNo { get; set; }
        /// <summary>
        /// Number transaction at vnpay
        /// </summary>
        public int TransactionNo { get; set; }
        /// <summary>
        /// Payment time, recorded at VNPAY in GMT+7
        /// </summary>
        public DateTime PayDate { get; set; }
        public int TransactionStatus { get; set; }
        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet{ get; set; }
        public int TransactionType { get; set; }
    }
}
