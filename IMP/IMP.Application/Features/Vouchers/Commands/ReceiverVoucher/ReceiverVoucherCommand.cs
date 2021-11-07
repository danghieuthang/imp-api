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

namespace IMP.Application.Features.Vouchers.Commands.ReceiverVoucher
{
    public class ReceiverVoucherCommand : ICommand<UserVoucherCodeViewModel>
    {
        public int CampaignVoucherId { get; set; }
    }

    public class ReceiverVoucherCommandHandler : CommandHandler<ReceiverVoucherCommand, UserVoucherCodeViewModel>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public ReceiverVoucherCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public override async Task<Response<UserVoucherCodeViewModel>> Handle(ReceiverVoucherCommand request, CancellationToken cancellationToken)
        {
            var campaignVoucher = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(x => x.Id == request.CampaignVoucherId,
                    x => x.Voucher, x => x.Voucher.VoucherCodes);

            if (campaignVoucher == null)
            {
                throw new ValidationException(new ValidationError("campaign_voucher_id", "Không tồn tại."));
            }

            var isExistVoucherForUser = await UnitOfWork.Repository<VoucherCodeApplicationUser>().FindSingleAsync(
                    predicate: x => x.VoucherCode.VoucherId == campaignVoucher.VoucherId && x.VoucherCode.QuantityUsed < x.VoucherCode.Quantity,
                     x => x.VoucherCode);
            // if this voucher code can use
            if (isExistVoucherForUser != null)
            {
                var voucherCodeView = Mapper.Map<UserVoucherCodeViewModel>(isExistVoucherForUser.VoucherCode);
                return new Response<UserVoucherCodeViewModel>(voucherCodeView);
            }

            // get another voucher code 
            var voucherCode = campaignVoucher.Voucher.VoucherCodes.Where(x => x.QuantityUsed < x.Quantity).FirstOrDefault();
            if (voucherCode != null)
            {
                var voucherCodeUser = new VoucherCodeApplicationUser
                {
                    VoucherCodeId = voucherCode.Id,
                    ApplicationUserId = _authenticatedUserService.ApplicationUserId
                };

                UnitOfWork.Repository<VoucherCodeApplicationUser>().Add(voucherCodeUser);
                await UnitOfWork.CommitAsync();

                var voucherCodeView = Mapper.Map<UserVoucherCodeViewModel>(voucherCode);
                return new Response<UserVoucherCodeViewModel>(voucherCodeView);
            }
            return new Response<UserVoucherCodeViewModel>(message: "Đã hết code");
        }
    }
}
