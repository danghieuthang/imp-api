using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.WalletTransactions.Commands.UpdateWalletTransaction
{
    public class CompletedWalletTransactionCommandValidator : AbstractValidator<CompletedWalletTransactionCommand>
    {
        public CompletedWalletTransactionCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.BankTranNo).MustRequired(256);
            RuleFor(x => x.BankId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<Bank>().IsExistAsync(x);
            });
            RuleFor(x=>x.Evidences).ListMustContainFewerThan(5);
            RuleForEach(x=>x.Evidences).ChildRules(evidence=>{
                evidence.RuleFor(x=>x.Url).MustValidUrl();
            });
        }
    }
}
