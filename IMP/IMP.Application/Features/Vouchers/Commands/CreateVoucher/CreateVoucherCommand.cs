using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IMP.Application.Features.Vouchers.Commands.CreateVoucher
{
    public class CreateVoucherCommand : ICommand<VoucherViewModel>
    {
        public string VoucherName { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }

        public class CreateVoucherCommandHandler : CommandHandler<CreateVoucherCommand, VoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CreateVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<VoucherViewModel>> Handle(CreateVoucherCommand request, CancellationToken cancellationToken)
            {
                var applicationUser = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == _authenticatedUserService.ApplicationUserId);

                var voucher = Mapper.Map<Voucher>(request);
                voucher.BrandId = applicationUser.BrandId.Value;

                await UnitOfWork.Repository<Voucher>().AddAsync(voucher);
                await UnitOfWork.CommitAsync();

                var voucherView = Mapper.Map<VoucherViewModel>(voucher);

                return new Response<VoucherViewModel>(voucherView);
            }
        }
    }
}