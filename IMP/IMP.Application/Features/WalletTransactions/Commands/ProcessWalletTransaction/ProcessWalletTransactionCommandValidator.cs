using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.WalletTransactions.Commands.ProcessWalletTransaction
{
    public class ProcessWalletTransactionCommandValidator : AbstractValidator<ProcessWalletTransactionCommand>
    {
        public ProcessWalletTransactionCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.AdminId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<ApplicationUser>().IsExistAsync(x);
            });
        }
    }
}