using AutoMapper;
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

namespace IMP.Application.Features.WalletTransactions.Commands.CancelWalletTransaction
{
    public class CancelWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int? ApplicationUserId { get; set; }
        public string Note { get; set; }
        [JsonIgnore]
        public int? AdminId { get; set; }
        public class CancelWalletTransactionCommandHandler : CommandHandler<CancelWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public CancelWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(CancelWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var walletTransaction = await _walletTransactionRepository.FindSingleAsync(x => x.Id == request.Id, includeProperties: x => x.WalletFrom);
                if (walletTransaction != null)
                {
                    // if status is not new and owner of transaction
                    if (walletTransaction.TransactionStatus != (int)WalletTransactionStatus.New && request.ApplicationUserId.HasValue)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Chỉ có thể hủy các giao dịch chưa xử lí."));
                    }
                    if (request.ApplicationUserId.HasValue && walletTransaction.WalletTo.ApplicationUserId != request.ApplicationUserId)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Không có quyền."));
                    }
                    if(request.ApplicationUserId.HasValue){ // if user cancel, set sender is user
                        walletTransaction.SenderId = request.ApplicationUserId;
                    }else{ // if admin cancel, set sender is user
                        walletTransaction.SenderId = request.AdminId;
                    }
                    walletTransaction.Note = request.Note;
                    walletTransaction.TransactionStatus = (int)WalletTransactionStatus.Cancelled;

                    _walletTransactionRepository.Update(walletTransaction);
                    await UnitOfWork.CommitAsync();


                    walletTransaction = await _walletTransactionRepository.FindSingleAsync(x => x.Id == request.Id, x => x.Sender, x => x.Receiver);
                    var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTransaction);
                    return new Response<WalletTransactionViewModel>(walletTransactionView);
                }
                return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Không tồn tại."));
            }
        }
    }
}
