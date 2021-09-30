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

namespace IMP.Application.Features.WalletTransactions.Commands
{
    public class WalletTranactionEvidence
    {
        public string Url { get; set; }
    }

    public class CompletedWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        public int Id { get; set; }
        public string BankTranNo { get; set; }
        public int BankId { get; set; }
        public List<WalletTranactionEvidence> Evidences { get; set; }
        [JsonIgnore]
        public int AdminId { get; set; }
        public class CompletedWalletTransactionCommandHandler : CommandHandler<CompletedWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public CompletedWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(CompletedWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var walletTransaction = await _walletTransactionRepository.GetByIdAsync(request.Id);
                if (walletTransaction != null)
                {
                    if (walletTransaction.TransactionStatus != (int)WalletTransactionStatus.Processing)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Chỉ có thể hoàn thành các giao dịch chưa xử lí."));
                    }

                    if (walletTransaction.SenderId != request.AdminId)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Giao dịch này đã bị nhân viên khác xử lí."));
                    }

                    // get withdraw wallet
                    var walletRepository = UnitOfWork.Repository<Wallet>();
                    var wallet = await walletRepository.GetByIdAsync(walletTransaction.WalletFromId.Value);
                    if (wallet != null)
                    {
                        wallet.Balance -= walletTransaction.Amount;
                        walletRepository.Update(wallet);
                    };

                    // Add evidences to transacion
                    if (request.Evidences.Count > 0)
                    {
                        walletTransaction.Evidences = JsonConvert.SerializeObject(request.Evidences);
                    }
                    // Update transaction info
                    walletTransaction.BankId = request.BankId;
                    walletTransaction.TransactionStatus = (int)WalletTransactionStatus.Successful;
                    walletTransaction.BankTranNo = request.BankTranNo;
                    walletTransaction.PayDate = DateTime.UtcNow;
                    walletTransaction.SenderBalance = wallet.Balance;

                    _walletTransactionRepository.Update(walletTransaction);

                    // Commit
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
