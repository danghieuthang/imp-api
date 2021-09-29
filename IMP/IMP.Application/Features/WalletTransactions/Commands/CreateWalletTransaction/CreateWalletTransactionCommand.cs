using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;

namespace IMP.Application.Features.WalletTransactions.Commands.CreateWalletTransaction
{
    public class CreateWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        public int Amount { get; set; }
        public string TransactionInfo { get; set; }
        public int ApplicationUserTo { get; set; }
        [JsonIgnore]
        public int ApplicationUserFrom { get; set; }
        public class CreateWalletTransactionCommandHandler : CommandHandler<CreateWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            private readonly IGenericRepository<Wallet> _walletRepository;
            public CreateWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
                _walletRepository = unitOfWork.Repository<Wallet>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(CreateWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var walletFrom = await _walletRepository.FindSingleAsync(x => x.ApplicationUserId == request.ApplicationUserFrom);
                var walletTo = await _walletRepository.FindSingleAsync(x => x.ApplicationUserId == request.ApplicationUserTo);
                var walletTransaction = new WalletTransaction
                {
                    Amount = request.Amount,
                    SenderId = request.ApplicationUserFrom,
                    ReceiverId = request.ApplicationUserTo,
                    TransactionInfo = request.TransactionInfo,
                    TransactionType = (int)TransactionType.Transfer,
                    TransactionStatus = (int)WalletTransactionStatus.Successful,
                    PayDate = DateTime.UtcNow,
                    WalletToId = walletTo.Id,
                    WalletFromId = walletFrom.Id,
                };
                // add wallet transction
                await _walletTransactionRepository.AddAsync(walletTransaction);
                // update wallet after transaction
                walletFrom.Balance -= request.Amount;
                walletTo.Balance += request.Amount;

                _walletRepository.Update(walletFrom);
                _walletRepository.Update(walletTo);
                // commit
                await UnitOfWork.CommitAsync();


                walletTransaction = await _walletTransactionRepository.FindSingleAsync(x => x.Id == walletTransaction.Id, x => x.Sender, x => x.Receiver);
                var walletTransactionView = Mapper.Map<WalletTransactionViewModel>(walletTransaction);
                return new Response<WalletTransactionViewModel>(walletTransactionView);
            }
        }
    }
}