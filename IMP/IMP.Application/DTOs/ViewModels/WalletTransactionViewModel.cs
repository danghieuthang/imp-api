using IMP.Application.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IMP.Application.Models.ViewModels
{
    public class WalletTransactionViewModel : BaseViewModel<int>
    {
        public decimal Amount { get; set; }
        public string TransactionInfo { get; set; }
        public string Note { get; set; }
        public int? BankId { get; set; }
        /// <summary>
        /// Number transaction at banking
        /// </summary>
        public string BankTranNo { get; set; }
        /// <summary>
        /// Number transaction at vnpay
        /// </summary>
        public string VnpTransactionNo { get; set; }
        /// <summary>
        /// Payment time, recorded at VNPAY in GMT+7
        /// </summary>
        public DateTime PayDate { get; set; }
        public WalletTransactionStatus TransactionStatus { get; set; }
        public TransactionUserViewModel Sender { get; set; }
        public TransactionUserViewModel Receiver { get; set; }
        public decimal? SenderBalance { get; set; }
        public decimal? ReceiverBalance { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<TransactionEvidence> Evidences { get; set; }
        public bool IsSender => Sender != null;
    }

    public class TransactionUserViewModel : BaseViewModel<int>
    {
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public BrandViewModel Brand { get; set; }
    }

    public class TransactionEvidence
    {
        public string Url { get; set; }
    }
}