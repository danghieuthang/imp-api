using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherCodes.Commands.UpdateVoucherCode
{
    public class VoucherCodeRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateListVoucherCodeCommand : ICommand<VoucherViewModel>
    {
        public int VoucherId { get; set; }
        public List<VoucherCodeRequest> VoucherCodes { get; set; }

        public class UpdateListVoucherCodeCommandHandler : CommandHandler<UpdateListVoucherCodeCommand, VoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<VoucherCode> _voucherCodeRepository;
            private readonly IGenericRepository<Voucher> _voucherRepository;
            public UpdateListVoucherCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _voucherCodeRepository = unitOfWork.Repository<VoucherCode>();
                _voucherRepository = unitOfWork.Repository<Voucher>();
            }

            public override async Task<Response<VoucherViewModel>> Handle(UpdateListVoucherCodeCommand request, CancellationToken cancellationToken)
            {
                var voucher = await _voucherRepository.FindSingleAsync(x => x.Id == request.VoucherId, x => x.VoucherCodes);
                if (voucher == null)
                {
                    throw new ValidationException(new ValidationError("voucher_id", "Không hợp lệ."));
                }

                if (voucher.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new ValidationException(new ValidationError("voucher_id", "Không có quyền."));
                }

                var requestVoucherCodes = request.VoucherCodes.Distinct(new VoucherCodeComparer()).ToList();

                var codes = voucher.VoucherCodes.ToList();

                codes.ForEach(x =>
                {
                    var requestCode = requestVoucherCodes.Where(y => y.Id == x.Id).FirstOrDefault();
                    if (requestCode != null)
                    {
                        x.Code = requestCode.Code.ToUpper();
                        x.Quantity = requestCode.Quantity;
                    }
                });

                voucher.VoucherCodes = codes;

                _voucherRepository.Update(voucher);

                await UnitOfWork.CommitAsync();

                var views = Mapper.Map<VoucherViewModel>(voucher);
                return new Response<VoucherViewModel>(views);


            }

            internal class VoucherCodeComparer : IEqualityComparer<VoucherCodeRequest>
            {
                public bool Equals(VoucherCodeRequest code1, VoucherCodeRequest code2)
                {
                    if (code1.Code.ToString().ToUpper() == code2.Code.ToString().ToUpper())
                    {
                        return true;
                    }
                    return false;
                }

                public int GetHashCode([DisallowNull] VoucherCodeRequest obj)
                {
                    return obj.Code.ToUpper().GetHashCode();
                }
            }
        }
    }
}
