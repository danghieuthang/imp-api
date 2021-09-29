using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;

namespace IMP.Application.Features.WalletTransactions.Commands.ProcessWalletTransaction
{
    public class ProcessWalletTransactionCommand : ICommand<WalletTransactionViewModel>
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public class ProcessWalletTransactionCommandHandler : CommandHandler<ProcessWalletTransactionCommand, WalletTransactionViewModel>
        {
            private readonly IGenericRepository<WalletTransaction> _walletTransactionRepository;
            public ProcessWalletTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _walletTransactionRepository = unitOfWork.Repository<WalletTransaction>();
            }

            public override async Task<Response<WalletTransactionViewModel>> Handle(ProcessWalletTransactionCommand request, CancellationToken cancellationToken)
            {
                var walletTransaction = await _walletTransactionRepository.GetByIdAsync(request.Id);

                if (walletTransaction != null)
                {
                    if (walletTransaction.TransactionStatus != (int)WalletTransactionStatus.New)
                    {
                        return new Response<WalletTransactionViewModel>(error: new Models.ValidationError("id", "Chỉ có thể xử lí giao dịch mới tạo."));
                    }
                    walletTransaction.SenderId = request.AdminId;
                    walletTransaction.TransactionStatus = (int)WalletTransactionStatus.Processing;
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