using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.WalletTransactions.Commands.CreateWalletTransaction
{
    public class CreateWalletTransactionCommandValidator : AbstractValidator<CreateWalletTransactionCommand>
    {
        public CreateWalletTransactionCommandValidator(IUnitOfWork unitOfWork)
        {
            var walletRepository = unitOfWork.Repository<Wallet>();

            RuleFor(x => x.ApplicationUserFrom).MustExistEntityId(async (x, y) =>
            {
                return await walletRepository.IsExistAsync(wallet => wallet.ApplicationUserId == x);
            }).MustAsync(async (x, y, z) =>
            {
                return await walletRepository.IsExistAsync(w => w.Balance >= x.Amount && w.ApplicationUserId == y);
            }).WithMessage("Số tiền trong ví của bạn không đủ.")
            .MustAsync(async (x, y, z) =>
            {
                var wallet = await walletRepository.FindSingleAsync(x => x.ApplicationUserId == y);
                if (wallet != null && x.ApplicationUserFrom == wallet.ApplicationUserId)
                {
                    return true;
                }
                return false;
            }).WithMessage("Không có quyền chuyển.");

            RuleFor(x => x.ApplicationUserTo).MustExistEntityId(async (x, y) =>
            {
                return await walletRepository.IsExistAsync(x);
            }).When(x => x.ApplicationUserFrom != x.ApplicationUserTo).WithMessage("Người gửi và người nhận bị trùng nhau.");
        }
    }
}