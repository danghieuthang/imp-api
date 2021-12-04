using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherTransactions.Commands.CreateVoucherTransaction
{
    public class CreateVoucherTransactionCommand : VoucherTransactionRequest, ICommand<VoucherTransactionViewModel>
    {
        public class CreateVoucherTranactionCommandHandler : CommandHandler<CreateVoucherTransactionCommand, VoucherTransactionViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CreateVoucherTranactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<VoucherTransactionViewModel>> Handle(CreateVoucherTransactionCommand request, CancellationToken cancellationToken)
            {
                var voucherCodeRepository = UnitOfWork.Repository<VoucherCode>();
                var voucherCode = await voucherCodeRepository.FindSingleAsync(x => x.Code.ToLower() == request.Code.ToLower() && x.VoucherId == request.VoucherId, x => x.Voucher);

                if (voucherCode == null)
                {
                    throw new ValidationException(new ValidationError("code", "Voucher code không tồn tại."));
                }

                if (voucherCode.Voucher.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("", "Không có quyền."));
                }
                if (voucherCode.QuantityUsed >= voucherCode.Quantity)
                {
                    throw new ValidationException(new ValidationError("code", "Voucher code đã sử dụng đủ."));
                }

                var voucherTransaction = Mapper.Map<VoucherTransaction>(request);
                voucherTransaction.VoucherCodeId = voucherCode.Id;

                UnitOfWork.Repository<VoucherTransaction>().Add(voucherTransaction);

                voucherCode.QuantityUsed++;
                voucherCodeRepository.Update(voucherCode);

                await UnitOfWork.CommitAsync();

                var voucherTransactionView = Mapper.Map<VoucherTransactionViewModel>(voucherTransaction);
                return new Response<VoucherTransactionViewModel>(voucherTransactionView);
            }
        }
    }
}
