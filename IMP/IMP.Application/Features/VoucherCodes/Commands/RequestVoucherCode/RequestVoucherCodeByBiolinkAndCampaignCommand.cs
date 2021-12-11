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
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink.ToLower() == request.Biolink.ToLower()));
                if (user == null)
                {
                    return new Response<bool>(false);
                }

                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.InfluencerId == user.Id);
                if (campaignMember == null)
                {
                    return new Response<bool>(false);
                }

                #region check voucher
                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(
                        predicate: x => x.VoucherId == request.VoucherId
                            && x.CampaignId == request.CampaignId
                            && x.IsBestInfluencerReward == false
                            && x.IsDefaultReward == false
                            && (x.Voucher.ToDate == null
                                || (x.Voucher.ToDate.HasValue && x.Voucher.ToDate.Value.CompareTo(DateTime.UtcNow) > 0) // Get a voucher that hasn't expired yet
                                ),
                        include: x => x.Include(y => y.Voucher));

                var voucher = vouchers?.Voucher;
                if (voucher == null)
                {
                    return new Response<bool>(message: "Mã giảm giá không tồn tại hoặc đã hết hạn.");
                }
                #endregion

                #region get voucher code
                var voucherCode = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync( // get voucher code that assigned for influencer and can used
                        predicate: x => x.VoucherId == voucher.Id
                            && ((x.CampaignMemberId.HasValue && x.CampaignMemberId.Value == campaignMember.Id))
                            && (
                                (x.Quantity == 1 && x.QuantityUsed == 0 && (x.Expired == null || (x.Expired.HasValue && x.Expired.Value.CompareTo(DateTime.UtcNow) < 0)))  // Voucher 1 lần sài && chưa ai sài && không bị ai giữ
                                || (x.Quantity > 1 && x.QuantityUsed < x.Quantity) // Voucher nhiều lần sài và còn sử dụng được
                               )
                    );

                if (voucherCode == null)
                {
                    // get voucher that not assigned for influencer and can used
                    voucherCode = await UnitOfWork.Repository<VoucherCode>().FindSingleAsync(
                       predicate: x => x.VoucherId == voucher.Id
                           && (x.CampaignMemberId == null)
                           && (
                               (x.Quantity == 1 && x.QuantityUsed == 0 && (x.Expired == null || (x.Expired.HasValue && x.Expired.Value.CompareTo(DateTime.UtcNow) < 0)))  // Voucher 1 lần sài && chưa ai sài && không bị ai giữ
                               || (x.Quantity > 1 && x.QuantityUsed < x.Quantity) // Voucher nhiều lần sài và còn sử dụng được
                              )
                   );
                }


                if (voucherCode == null)
                {
                    return new Response<bool>(message: "Đã hết mã giảm giá.");
                }
                #endregion

                #region update quantity get for voucher influencer
                voucherCode.QuantityGet++;

                if (voucher.Quantity == 1 && voucher.HoldTime.HasValue)
                {
                    voucherCode.Expired = DateTime.UtcNow.Add(voucher.HoldTime.Value);
                }

                if (voucherCode.CampaignMemberId == null)
                {
                    voucherCode.CampaignMemberId = campaignMember.Id;

                }

                UnitOfWork.Repository<VoucherCode>().Update(voucherCode);
                #endregion


                if (user.VoucherCodeApplicationUsers.Any(x => x.VoucherCodeId == voucherCode.Id))
                {
                    return new Response<bool>(true, "Bạn đã code ở chiến dịch này và vẫn còn có thể sử dụng.");
                }

                VoucherCodeApplicationUser voucherCodeApplicationUser = new VoucherCodeApplicationUser
                {
                    ApplicationUserId = _authenticatedUserService.ApplicationUserId,
                    VoucherCodeId = voucher.Id,
                };

                UnitOfWork.Repository<VoucherCodeApplicationUser>().Add(voucherCodeApplicationUser);

                await UnitOfWork.CommitAsync();
                return new Response<bool>(true);
            }
        }
    }
}
