using AutoMapper;
using FluentValidation;
using IMP.Application.Constants;
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
        public decimal Amount { get; set; }
        public class RequestWithdrawWalletCommandValidator: AbstractValidator<RequestWithdrawWalletCommand>
        {
            public RequestWithdrawWalletCommandValidator()
            {
                RuleFor(x => x.Amount).GreaterThanOrEqualTo(50000).WithMessage("Tiền rút phải lớn hơn hoặc bằng 50000").WithErrorCode(ErrorConstants.Application.WalletTransaction.AmountNotValid.ToString());
            }
        }
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
                    return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("application_user_id", "Đang có 1 yêu cầu rút tiền đang đợi xử lí."), code: ErrorConstants.Application.WalletTransaction.ExistWithdrawRequest);
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
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("amount", "Tiền rút phải nhỏ hơn hoặc bằng tiền trong ví."), code: ErrorConstants.Application.WalletTransaction.BalanceNotEnough);
                    };
                }
                return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("application_user_id", "Tài khoản chưa có ví."));
            }
        }
    }
}
