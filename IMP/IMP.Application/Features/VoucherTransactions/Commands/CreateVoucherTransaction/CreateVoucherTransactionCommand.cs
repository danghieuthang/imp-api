using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;
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
                var voucherInfluencerRepository = UnitOfWork.Repository<VoucherInfluencer>();

                var campaignVoucher = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.Voucher.VoucherCodes.Any(y => y.Code.ToUpper() == request.VoucherCode.ToUpper()), x => x.Campaign);
                if (campaignVoucher == null)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Voucher code không tồn tại."));
                }

                var voucherCode = await voucherCodeRepository.FindSingleAsync(x => x.Code.ToLower() == request.VoucherCode.ToLower() && x.VoucherId == campaignVoucher.VoucherId, x => x.Voucher);
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

                #region update voucher influencer
                //var voucherInfluencer = await voucherInfluencerRepository.FindSingleAsync(x => x.VoucherId == campaignVoucher.VoucherId && x.InfluencerId == request.InfluencerId);
                //if (voucherInfluencer != null)
                //{
                //    voucherInfluencer.QuantityUsed++;
                //    voucherInfluencerRepository.Update(voucherInfluencer);
                //}

                //var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == voucherCode.CampaignMemberId);
                //if (campaignVoucher.Campaign.VoucherCommissionMode == (int)VoucherCommissionType.Order) // if earning per oder
                //{
                //    List<VoucherCommissionPrices> prices = JsonConvert.DeserializeObject<List<VoucherCommissionPrices>>(campaignVoucher.Campaign.VoucherCommissionPrices);
                //    if (campaignVoucher.Campaign.IsPercentVoucherCommission) // if earn by percent of order
                //    {
                //        campaignMember.Money += prices.FirstOrDefault().Value * request.TotalProductAmount / (decimal)100.0;
                //    }
                //    else // earn by interval of order
                //    {
                //        foreach (var price in prices)
                //        {
                //            if (request.TotalPrice >= price.From && (request.TotalPrice <= price.To || price.To == null))
                //            {
                //                campaignMember.Money += price.Value;
                //                break;
                //            }
                //        }
                //    }
                //}
                //else if (campaignVoucher.Campaign.VoucherCommissionMode == (int)VoucherCommissionType.Product)// if earning by product
                //{
                //    List<VoucherCommissionPrices> prices = JsonConvert.DeserializeObject<List<VoucherCommissionPrices>>(campaignVoucher.Campaign.VoucherCommissionPrices);
                //    if (campaignVoucher.Campaign.IsPercentVoucherCommission) // if earn by percent
                //    {
                //        campaignMember.Money += prices.FirstOrDefault().Value * request.TotalDiscount / (decimal)100.0;
                //    }
                //}

                //UnitOfWork.Repository<CampaignMember>().Update(campaignMember);
                #endregion
                await UnitOfWork.CommitAsync();

                var voucherTransactionView = Mapper.Map<VoucherTransactionViewModel>(voucherTransaction);
                return new Response<VoucherTransactionViewModel>(voucherTransactionView);
            }
        }
    }
}
