using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.WalletTransactions.Commands
{
    public class CompletedWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        public int Id { get; set; }
        public string BankTranNo { get; set; }
        public class CompletedWalletTransactionCommandHandler : CommandHandler<CompletedWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public CompletedWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(CompletedWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var walletTranaction = await _walletTransactionRepository.GetByIdAsync(request.Id);
                if (walletTranaction != null)
                {
                    if (walletTranaction.TransactionStatus != (int)WalletTransactionStatus.Processing)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Chỉ có thể hoàn thành các giao dịch chưa xử lí."));
                    }
                    walletTranaction.TransactionStatus = (int)WalletTransactionStatus.Successful;
                    walletTranaction.BankTranNo = request.BankTranNo;
                    walletTranaction.PayDate = DateTime.UtcNow;
                    _walletTransactionRepository.Update(walletTranaction);
                    await UnitOfWork.CommitAsync();
                    _ = await SubtractWalletAfterWithdraw(walletTranaction.WalletId, walletTranaction.Amount);
                    var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTranaction);
                    return new Response<WalletTransactionViewModel>(walletTransactionView);
                }
                return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Không tồn tại."));
            }

            private async Task<bool> SubtractWalletAfterWithdraw(int walletId, decimal amount)
            {
                var walletRepository = UnitOfWork.Repository<Wallet>();
                var wallet = await walletRepository.GetByIdAsync(walletId);
                if (wallet != null)
                {
                    wallet.Balance -= amount;
                    walletRepository.Update(wallet);
                    await UnitOfWork.CommitAsync();
                    return true;
                };
                return false;
            }
        }
    }
}
