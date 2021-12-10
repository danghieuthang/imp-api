using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.VoucherTransactions.Queries.GetTransactionReportOfVoucher
{
    public class TransactionReportOfVoucherQuery : IQuery<VoucherTransactionReportOfVoucherViewModel>
    {
        [FromQuery(Name = "campaign_member_id")]
        public int CampaignMemberId { get; set; }
        [FromQuery(Name = "voucher_id")]
        public int VoucherId { get; set; }
        public class TransactionReportOfVoucherQueryHandler : QueryHandler<TransactionReportOfVoucherQuery, VoucherTransactionReportOfVoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public TransactionReportOfVoucherQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<VoucherTransactionReportOfVoucherViewModel>> Handle(TransactionReportOfVoucherQuery request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == request.CampaignMemberId,
                       include: x => x.Include(y => y.Campaign).Include(y => y.VoucherCodes.Where(v => v.VoucherId == request.VoucherId)).ThenInclude(y => y.VoucherTransactions));
                if (campaignMember == null)
                {
                    throw new KeyNotFoundException();
                }

                if (campaignMember.InfluencerId != _authenticatedUserService.ApplicationUserId)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không có quyền."));
                }

                var report = new VoucherTransactionReportOfVoucherViewModel
                {
                    QuantitiyGet = campaignMember.VoucherCodes.Sum(x => x.QuantityGet),
                    QuantitiyUsed = campaignMember.VoucherCodes.Sum(x => x.QuantityUsed),
                    TotalTransaction = campaignMember.VoucherCodes.Sum(x => x.VoucherTransactions.Count),
                    TotalVoucherCode = campaignMember.VoucherCodes.Count(),
                    TotalProductAmount = campaignMember.VoucherCodes.Sum(x => x.VoucherTransactions.Sum(y => y.TotalProductAmount)),
                    TotalEarningAmount = campaignMember.VoucherCodes.Sum(x => x.VoucherTransactions.Sum(y => TransactionUtils.CaculateMoneyEarnFromTransaction(campaignMember.Campaign, y))),
                };
                return new Response<VoucherTransactionReportOfVoucherViewModel>(report);

            }
        }
    }
}
