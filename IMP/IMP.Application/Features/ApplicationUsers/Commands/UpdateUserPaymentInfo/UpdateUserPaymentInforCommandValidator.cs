using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Commands.UpdateUserPaymentInfo
{
    public class UpdateUserPaymentInforCommandValidator : AbstractValidator<UpdateUserPaymentInfoCommand>
    {
        private readonly IGenericRepository<Bank> _bankRepository;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;
        public UpdateUserPaymentInforCommandValidator(IUnitOfWork unitOfWork)
        {
            _bankRepository = unitOfWork.Repository<Bank>();
            _applicationUserRepository = unitOfWork.Repository<ApplicationUser>();

            RuleFor(x => x.BankId).MustExistEntityId(IsExistBank);
            RuleFor(x => x.ApplicationUserId).MustExistEntityId(IsExistApplicationUser);

            RuleFor(x => x.AccountNumber).MustValidBankAccountNumber();
            RuleFor(x => x.AccountName).MustRequired(256);
        }

        private async Task<bool> IsExistApplicationUser(int id, CancellationToken cancellationToken)
        {
            return await _applicationUserRepository.IsExistAsync(id);
        }
        private async Task<bool> IsExistBank(int id, CancellationToken cancellationToken)
        {
            return await _bankRepository.IsExistAsync(id);
        }

    }
}
