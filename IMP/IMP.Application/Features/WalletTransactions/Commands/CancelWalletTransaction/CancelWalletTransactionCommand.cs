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

namespace IMP.Application.Features.WalletTransactions.Commands.CancelWalletTransaction
{
    public class CancelWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        public int Id { get; set; }
        public int? ApplicationUserId { get; set; }
        public class CancelWalletTransactionCommandHandler : CommandHandler<CancelWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public CancelWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(CancelWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var walletTranaction = await _walletTransactionRepository.FindSingleAsync(x => x.Id == request.Id, includeProperties: x => x.Wallet);
                if (walletTranaction != null)
                {
                    if (walletTranaction.TransactionStatus != (int)WalletTransactionStatus.Processing)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Chỉ có thể hủy các giao dịch chưa xử lí."));
                    }
                    if (request.ApplicationUserId.HasValue && walletTranaction.Wallet.ApplicationUserId != request.ApplicationUserId)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Không có quyền."));
                    }
                    walletTranaction.TransactionStatus = (int)WalletTransactionStatus.Cancelled;
                    _walletTransactionRepository.Update(walletTranaction);
                    await UnitOfWork.CommitAsync();
                    var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTranaction);
                    return new Response<WalletTransactionViewModel>(walletTransactionView);
                }
                return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Không tồn tại."));
            }
        }
    }
}
