using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherCodes.Commands.RequestVoucherCode
{
    public class RequestVoucherCodeByBiolinkAndCampaignCommand : ICommand<bool>
    {
        public int CampaignId { get; set; }
        public int VoucherId { get; set; }
        public string Biolink { get; set; }
        public class RequetVoucherCodeByBioLinkAndCampaignCommand : CommandHandler<RequestVoucherCodeByBiolinkAndCampaignCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public RequetVoucherCodeByBioLinkAndCampaignCommand(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<bool>> Handle(RequestVoucherCodeByBiolinkAndCampaignCommand request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink.ToLower() == request.Biolink.ToLower()), x => x.VoucherCodeApplicationUsers);
                if (user == null)
                {
                    return new Response<bool>(false);
                }

                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.VoucherId == request.VoucherId
                            && x.CampaignId == request.CampaignId
                            && x.IsBestInfluencerReward == false
                            && x.IsDefaultReward == false,
                        selector: x => x.VoucherId).ToListAsync();

                var voucherCodes = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => x.CampaignMember.InfluencerId == user.Id
                            && x.CampaignMember.CampaignId == request.CampaignId
                            && x.VoucherId == request.VoucherId
                            && x.QuantityUsed < x.Quantity
                            && (x.Voucher.ToDate == null || x.Voucher.ToDate > DateTime.UtcNow)
                            ).Take(1).ToListAsync();
                if (!voucherCodes.Any())
                {
                    return new Response<bool>(message: "Đã hết voucher code.");
                }

                if (user.VoucherCodeApplicationUsers.Any(x => x.VoucherCodeId == voucherCodes.FirstOrDefault().Id))
                {
                    return new Response<bool>(true, "Bạn đã code ở chiến dịch này và vẫn còn có thể sử dụng.");
                }

                VoucherCodeApplicationUser voucherCodeApplicationUser = new VoucherCodeApplicationUser
                {
                    ApplicationUserId = _authenticatedUserService.ApplicationUserId,
                    VoucherCodeId = voucherCodes.FirstOrDefault().Id,
                };

                UnitOfWork.Repository<VoucherCodeApplicationUser>().Add(voucherCodeApplicationUser);

                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
