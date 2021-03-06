using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace IMP.Application.Features.WalletTransactions.Commands.ConfirmVnpWalletTransaction
{
    public class ConfirmVnpWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        [FromQuery(Name = "vnp_TmnCode")]
        public string VnP_TmnCode { get; set; }
        [FromQuery(Name = "vnp_TxnRef")]
        public string Vnp_TxnRef { get; set; }
        [FromQuery(Name = "vnp_Amount")]
        public decimal Vnp_Amount { get; set; }
        [FromQuery(Name = "vnp_CardType")]
        public string Vnp_CardType { get; set; }
        [FromQuery(Name = "vnp_OrderInfo")]
        public string Vnp_OrderInfo { get; set; }
        [FromQuery(Name = "vnp_ResponseCode")]
        public string Vnp_ResponseCode { get; set; }
        [FromQuery(Name = "vnp_BankCode")]
        public string Vnp_BankCode { get; set; }
        [FromQuery(Name = "vnp_BankTranNo")]
        public string Vnp_BankTranNo { get; set; }
        [FromQuery(Name = "vnp_PayDate")]
        public string Vnp_PayDate { get; set; }
        [FromQuery(Name = "vnp_TransactionNo")]
        public string Vnp_TransactionNo { get; set; }
        [FromQuery(Name = "vnp_TransactionStatus")]
        public string Vnp_TransactionStatus { get; set; }
        [FromQuery(Name = "vnp_SecureHashType")]
        public string Vnp_SecureHashType { get; set; }
        [FromQuery(Name = "vnp_SecureHash")]
        public string Vnp_SecureHash { get; set; }


        public class ConfirmVnpWalletTransactionCommandHandler : CommandHandler<ConfirmVnpWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IVnPayService _vnPayService;
            public ConfirmVnpWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IVnPayService vnPayService) : base(unitOfWork, mapper)
            {
                _vnPayService = vnPayService;
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(ConfirmVnpWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var data = new Dictionary<string, string>();

                data.Add("vnp_TmnCode", request.VnP_TmnCode);
                data.Add("vnp_TxnRef", request.Vnp_TxnRef);
                data.Add("vnp_Amount", request.Vnp_Amount.ToString());
                data.Add("vnp_CardType", request.Vnp_CardType);
                data.Add("vnp_OrderInfo", request.Vnp_OrderInfo);
                data.Add("vnp_ResponseCode", request.Vnp_ResponseCode);
                data.Add("vnp_BankCode", request.Vnp_BankCode);
                data.Add("vnp_BankTranNo", request.Vnp_BankTranNo);
                data.Add("vnp_PayDate", request.Vnp_PayDate);
                data.Add("vnp_TransactionNo", request.Vnp_TransactionNo);
                data.Add("vnp_TransactionStatus", request.Vnp_TransactionStatus);
                data.Add("vnp_SecureHashType", request.Vnp_SecureHashType);

                var isVerify = await _vnPayService.VerifyPaymentTransaction(data, request.Vnp_SecureHash);
                if (isVerify)
                {
                    if (!await IsExistWalletTransactionNo(request.Vnp_TransactionNo, cancellationToken))
                    {
                        int walletId = 0;
                        if (int.TryParse(request.Vnp_TxnRef.Split("_").Last(), out walletId))
                        {
                            var walletRepository = UnitOfWork.Repository<Wallet>();

                            var wallet = await walletRepository.GetByIdAsync(walletId);
                            //Update wallet balance
                            wallet.Balance += request.Vnp_Amount / 100;

                            var bank = await UnitOfWork.Repository<Bank>().FindSingleAsync(x => x.Code.ToLower() == request.Vnp_BankCode.ToLower());

                            DateTime payDate;
                            _ = DateTime.TryParseExact(request.Vnp_PayDate,
                                format: "yyyyMMddHHmmss",
                                provider: System.Globalization.CultureInfo.InvariantCulture,
                                style: System.Globalization.DateTimeStyles.None,
                                out payDate);
                            int status;
                            _ = int.TryParse(request.Vnp_TransactionStatus, out status);

                            var walletTransaction = new WalletTransaction
                            {
                                WalletToId = walletId,
                                Amount = request.Vnp_Amount / 100,
                                BankId = bank.Id,
                                PayDate = payDate,
                                BankTranNo = request.Vnp_BankTranNo,
                                TransactionInfo = request.Vnp_OrderInfo,
                                TransactionStatus = status == 0 ? (int)WalletTransactionStatus.Successful : (int)WalletTransactionStatus.Failure,
                                VnpTransactionNo = request.Vnp_TransactionNo,
                                TransactionType = (int)TransactionType.Recharge,
                                ReceiverId = wallet.ApplicationUserId,
                                SenderId = wallet.ApplicationUserId,
                                ReceiverBalance = wallet.Balance,
                            };

                            await UnitOfWork.Repository<WalletTransaction>().AddAsync(walletTransaction);

                            walletRepository.Update(wallet);

                            // Commit transaction
                            await UnitOfWork.CommitAsync();

                            // Recharge to wallet
                            //await RechargeWalletAfterTransactionVerified(request.Vnp_Amount / 100, walletId);

                            var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTransaction);
                            return new Response<WalletTransactionViewModel>(walletTransactionView);
                        }
                    }

                    return new Response<WalletTransactionViewModel>(message: "Giao dịch đã tồn tại.");

                }
                return new Response<WalletTransactionViewModel>(message: "Giao dịch không hợp lệ.");
            }
            private async Task<bool> IsExistWalletTransactionNo(string transactionNo, CancellationToken cancellationToken)
            {
                return await UnitOfWork.Repository<WalletTransaction>().IsExistAsync(x => x.VnpTransactionNo == transactionNo);
            }

            /// <summary>
            /// Recharge money to wallet after transaction verified
            /// </summary>
            /// <param name="amount">Amount(VND)</param>
            /// <param name="walletId">The id of wallet</param>
            private async Task RechargeWalletAfterTransactionVerified(decimal amount, int walletId)
            {
                var walletRepository = UnitOfWork.Repository<Wallet>();

                var wallet = await walletRepository.GetByIdAsync(walletId);
                wallet.Balance += amount;
                walletRepository.Update(wallet);

                //await UnitOfWork.CommitAsync();
            }
        }
    }

}
