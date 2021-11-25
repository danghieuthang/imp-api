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

namespace IMP.Application.Features.VoucherCodes.Commands.UpdateVoucherCode
{
    public class UpdateVoucherCodeCommand : ICommand<VoucherCodeViewModel>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        internal class UpdateVoucherCodeCommandHandler : CommandHandler<UpdateVoucherCodeCommand, VoucherCodeViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public UpdateVoucherCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<VoucherCodeViewModel>> Handle(UpdateVoucherCodeCommand request, CancellationToken cancellationToken)
            {
                var code = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync(x => x.Id == request.Id, x => x.Voucher);
                if (code == null)
                {
                    throw new ValidationException(new ValidationError("id", "Không tồn tại."));
                }
                if (code.Voucher.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("id", "Không có quyền."));
                }
                if (await UnitOfWork.Repository<VoucherCode>().IsExistAsync(x => x.VoucherId == request.Id && x.Code.ToUpper() == request.Code.ToUpper()))
                {
                    throw new ValidationException(new ValidationError("code", "Đã tồn tại trong voucher này."));
                }

                code.Code = request.Code;
                code.Quantity = request.Quantity;

                UnitOfWork.Repository<VoucherCode>().Update(code);
                await UnitOfWork.CommitAsync();

                var codeView = Mapper.Map<VoucherCodeViewModel>(code);
                return new Response<VoucherCodeViewModel>(codeView);
            }
        }
    }
}
