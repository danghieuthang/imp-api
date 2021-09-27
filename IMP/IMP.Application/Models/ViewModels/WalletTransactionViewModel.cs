using IMP.Application.Enums;
using System;

namespace IMP.Application.Models.ViewModels
{
    public class WalletTransactionViewModel : BaseViewModel<int>
    {
        public decimal Amount { get; set; }
        public string TransactionInfo { get; set; }
        public int? BankId { get; set; }
        /// <summary>
        /// Number transaction at banking
        /// </summary>
        public string BankTranNo { get; set; }
        /// <summary>
        /// Number transaction at vnpay
        /// </summary>
        public string TransactionNo { get; set; }
        /// <summary>
        /// Payment time, recorded at VNPAY in GMT+7
        /// </summary>
        public DateTime PayDate { get; set; }
        public WalletTransactionStatus TransactionStatus { get; set; }
        public int WalletId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}