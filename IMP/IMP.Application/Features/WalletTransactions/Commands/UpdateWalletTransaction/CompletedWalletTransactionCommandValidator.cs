using FluentValidation;
using IMP.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.WalletTransactions.Commands.UpdateWalletTransaction
{
    public class CompletedWalletTransactionCommandValidator: AbstractValidator<CompletedWalletTransactionCommand>
    {
        public CompletedWalletTransactionCommandValidator()
        {
            RuleFor(x => x.BankTranNo).MustRequired(256);   
        }
    }
}
