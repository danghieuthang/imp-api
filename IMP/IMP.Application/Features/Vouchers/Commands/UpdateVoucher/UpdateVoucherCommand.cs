using System.Windows.Input;
using System;
using Newtonsoft.Json;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using System.Threading.Tasks;
using IMP.Application.Wrappers;
using System.Threading;
using AutoMapper;
using IMP.Domain.Entities;
using IMP.Application.Exceptions;
using IMP.Application.Models;
using IMP.Application.Enums;
using System.Collections.Generic;

namespace IMP.Application.Features.Vouchers.Commands.UpdateVoucher
{
    public class UpdateVoucherCommand : ICommand<VoucherViewModel>
    {
        public int Id { get; set; }
        public string VoucherName { get; set; }
        public string Code { get; set; }
        public bool OnlyforInfluencer { get; set; }
        public bool OnlyforCustomer { get; set; }
        public decimal DiscountValue { get; set; }
        public DiscountValueType DiscountValueType { get; set; }
        public List<DiscountProductRequest> DiscountProducts { get; set; }

        public int Quantity { get; set; }
        public string Image { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }
        public bool UseForReward { get; set; }

        public TimeSpan? HoldTime { get; set; }

        public class UpdateVoucherCommandHandler : CommandHandler<UpdateVoucherCommand, VoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public UpdateVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<VoucherViewModel>> Handle(UpdateVoucherCommand request, CancellationToken cancellationToken)
            {
                var applicationUser = await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(_authenticatedUserService.ApplicationUserId);
                var repository = UnitOfWork.Repository<Voucher>();
                var voucher = await repository.FindSingleAsync(x => x.Id == request.Id, x => x.VoucherCodes);
                if (voucher.BrandId != applicationUser.BrandId)
                {
                    throw new ValidationException(new ValidationError("", $"Kh??ng c?? s???a th??ng tin."));
                }
                if (voucher != null)
                {
                    var voucherCodeRepository = UnitOfWork.Repository<VoucherCode>();

                    Mapper.Map(request, voucher);
                    repository.Update(voucher);

                    // Update all voucher code
                    foreach (var code in voucher.VoucherCodes)
                    {
                        code.Quantity = voucher.Quantity;
                        voucherCodeRepository.Update(code);
                    }
                    await UnitOfWork.CommitAsync();

                    var voucherView = Mapper.Map<VoucherViewModel>(voucher);
                    return new Response<VoucherViewModel>(voucherView);
                }
                throw new ValidationException(new ValidationError("id", $"'{request.Id}' kh??ng t???n t???i."));
            }
        }
    }
}