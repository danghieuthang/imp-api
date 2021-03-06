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
        public string SecretKey { get; set; }
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

                var campaignVoucher = await UnitOfWork.Repository<CampaignVoucher>().FindSingleAsync(x => x.CampaignId == request.CampaignId && x.Voucher.VoucherCodes.Any(y => y.Code.ToUpper() == request.Code.ToUpper()), x => x.Campaign);
                if (campaignVoucher == null)
                {
                    throw new ValidationException(new ValidationError("campaign_id", "Voucher code không tồn tại."));
                }

                var voucherCode = await voucherCodeRepository.FindSingleAsync(x => x.Code.ToLower() == request.Code.ToLower() && x.VoucherId == campaignVoucher.VoucherId, x => x.Voucher, x => x.Voucher.Brand);
                if (voucherCode == null)
                {
                    throw new ValidationException(new ValidationError("code", "Voucher code không tồn tại."));
                }

                if (voucherCode.Voucher.Brand.SecretKey != request.SecretKey)
                {
                    throw new ValidationException(new ValidationError("secret_key", "Mã bảo mật không hợp lệ."));
                }
                if (voucherCode.QuantityUsed >= voucherCode.Quantity)
                {
                    throw new ValidationException(new ValidationError("code", "Voucher code đã sử dụng đủ."));
                }
                if (voucherCode.CampaignMemberId == null)
                {
                    throw new ValidationException(new ValidationError("code", "Chưa có influencer được assign cho code này."));
                }
                #region create voucher transaction
                var voucherTransaction = Mapper.Map<VoucherTransaction>(request);

                decimal earningMoney = TransactionUtils.CaculateMoneyEarnFromTransaction(campaignVoucher.Campaign, voucherTransaction);

                voucherTransaction.VoucherCodeId = voucherCode.Id;
                voucherTransaction.Status = "Thành công";
                voucherTransaction.EarningMoney = earningMoney;
                UnitOfWork.Repository<VoucherTransaction>().Add(voucherTransaction);
                #endregion

                #region update voucher code after used
                voucherCode.QuantityUsed++;
                voucherCodeRepository.Update(voucherCode);
                #endregion

                #region update money earning for campaign member
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == voucherCode.CampaignMemberId);
                if (campaignMember != null)
                {
                    campaignMember.Money += earningMoney;
                    UnitOfWork.Repository<CampaignMember>().Update(campaignMember);
                }


                #endregion

                await UnitOfWork.CommitAsync();

                var voucherTransactionView = Mapper.Map<VoucherTransactionViewModel>(voucherTransaction);
                return new Response<VoucherTransactionViewModel>(voucherTransactionView);
            }
        }
    }
}
