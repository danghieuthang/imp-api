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

namespace IMP.Application.Features.WalletTransactions.Commands
{
    public class CreateWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        [FromQuery(Name = "vnp_TmnCode")]
        public string VnP_TmnCode { get; set; }
        [FromQuery(Name = "vnp_TxnRef")]
        public string Vnp_TxnRef { get; set; }
        [FromQuery(Name = "vnp_Amount")]
        public decimal Vnp_Amount { get; set; }
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


        public class CreateWalletTransactionCommandHandler : CommandHandler<CreateWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IVnPayService _vnPayService;
            public CreateWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IVnPayService vnPayService) : base(unitOfWork, mapper)
            {
                _vnPayService = vnPayService;
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(CreateWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var data = new Dictionary<string, string>();

                data.Add("vnp_TmnCode", request.VnP_TmnCode);
                data.Add("vnp_TxnRef", request.Vnp_TxnRef);
                data.Add("vnp_Amount", request.Vnp_Amount.ToString());
                data.Add("vnp_OrderInfo", request.Vnp_OrderInfo);
                data.Add("vnp_ResponseCode", request.Vnp_ResponseCode);
                data.Add("vnp_BankCode", request.Vnp_BankCode);
                data.Add("vnp_BankTranNo", request.Vnp_BankTranNo);
                data.Add("vnp_PayDate", request.Vnp_PayDate);
                data.Add("vnp_TransactionNo", request.Vnp_TransactionNo);
                data.Add("vnp_TransactionStatus", request.Vnp_TransactionStatus);
                data.Add("vnp_SecureHashType", request.Vnp_SecureHashType);

                var isVerify = _vnPayService.VerifyPaymentTransaction(data, request.Vnp_SecureHash);
                if (isVerify)
                {
                    int walletId = 0;
                    if (int.TryParse(request.Vnp_TxnRef.Split("_").Last(), out walletId))
                    {
                        var bank = await UnitOfWork.Repository<Bank>().FindSingleAsync(x => x.Code.ToLower() == request.Vnp_BankCode.ToLower());

                        DateTime payDate;
                        _ = DateTime.TryParseExact(request.Vnp_PayDate,
                            format: "yyyyMMddHHmmss",
                            provider: System.Globalization.CultureInfo.InvariantCulture,
                            style: System.Globalization.DateTimeStyles.None,
                            out payDate);
                        int status;
                        _ =  int.TryParse(request.Vnp_TransactionStatus, out status);

                        var walletTransaction = new WalletTransaction
                        {
                            WalletId = walletId,
                            Amount = request.Vnp_Amount,
                            BankId = bank.Id,
                            PayDate = payDate,
                            BankTranNo = request.Vnp_BankTranNo,
                            TransactionInfo = request.Vnp_OrderInfo,
                            TransactionStatus = status,
                            TransactionNo = request.Vnp_TransactionNo,
                            TransactionType = (int)TransactionType.Recharge
                        };
                        await UnitOfWork.Repository<WalletTransaction>().AddAsync(walletTransaction);
                        await UnitOfWork.CommitAsync();

                        var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTransaction);
                        return new Response<WalletTransactionViewModel>(walletTransactionView);
                    }
                }
                return new Response<WalletTransactionViewModel>(message: "Giao dịch không hợp lệ.");
            }
        }
    }

}
