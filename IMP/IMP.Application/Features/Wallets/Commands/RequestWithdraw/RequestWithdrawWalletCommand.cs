﻿using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Wallets.Commands.RequestWithdraw
{

    public class RequestWithdrawWalletCommand : ICommand<WalletTransactionViewModel>
    {

        [JsonIgnore]
        public int ApplicationUserId { get; set; }
        public int Amount { get; set; }
        public class RequestWithdrawWalletCommandHandler : CommandHandler<RequestWithdrawWalletCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public RequestWithdrawWalletCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(RequestWithdrawWalletCommand request, CancellationToken cancellationToken)
            {
                if (await _walletTransactionRepository.IsExistAsync(x =>
                     x.ReceiverId == request.ApplicationUserId
                     && (x.TransactionStatus == (int)WalletTransactionStatus.New 
                     || x.TransactionStatus == (int)WalletTransactionStatus.Processing)
                     ))
                {
                    return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("application_user_id", "Đang có 1 yêu cầu rút tiền đang đợi xuử lí."));
                }
                var wallet = await UnitOfWork.Repository<Wallet>().FindSingleAsync(x => x.ApplicationUserId == request.ApplicationUserId);
                if (wallet != null)
                {
                    if (wallet.Balance >= request.Amount)
                    {
                        var walletTransaction = new WalletTransaction
                        {
                            Amount = request.Amount,
                            TransactionType = (int)TransactionType.Withdrawal,
                            TransactionStatus = (int)WalletTransactionStatus.New,
                            WalletFromId = wallet.Id,
                            TransactionInfo = "Rút tiền",
                            ReceiverId = request.ApplicationUserId
                        };
                        await UnitOfWork.Repository<WalletTransaction>().AddAsync(walletTransaction);
                        await UnitOfWork.CommitAsync();
                        var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTransaction);
                        return new Response<WalletTransactionViewModel>(walletTransactionView);
                    }
                    else
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("amount", "Tiền rút phải nhỏ hơn hoặc bằng tiền trong ví."));
                    };
                }
                return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("application_user_id", "Tài khoản chưa có ví."));
            }
        }
    }
}