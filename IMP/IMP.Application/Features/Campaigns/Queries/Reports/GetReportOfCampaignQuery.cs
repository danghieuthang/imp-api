using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
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

namespace IMP.Application.Features.Campaigns.Queries.Reports
{
    public class GetReportOfCampaignQuery : IQuery<CampaignReportViewModel>
    {
        public int CampaignId { get; set; }
        public class GetReportOfCampaignQueryHandler : QueryHandler<GetReportOfCampaignQuery, CampaignReportViewModel>
        {
            public GetReportOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CampaignReportViewModel>> Handle(GetReportOfCampaignQuery request, CancellationToken cancellationToken)
            {
                CampaignReportViewModel report = new CampaignReportViewModel();
                var influencers = await UnitOfWork.Repository<CampaignMember>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => new { x.Id, x.Status }).ToListAsync();

                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => x.Voucher).Distinct().ToListAsync();

                List<int> voucherIds = vouchers.Select(x => x.Id).ToList();

                var voucherCodes = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => voucherIds.Contains(x.VoucherId)
                        ).ToListAsync();

                int totalVoucherCodeQuantity = voucherCodes.Sum(x => x.Quantity);
                // get voucher codes of only this campaign
                voucherCodes = voucherCodes.Where(x => x.CampaignMemberId.HasValue && influencers.Select(y => y.Id).Contains(x.CampaignMemberId.Value)).ToList();

                var transactions = await UnitOfWork.Repository<VoucherTransaction>().GetAll(
                    predicate: x => voucherIds.Contains(x.VoucherCode.VoucherId),
                    include: x => x.Include(y => y.VoucherCode)
                    ).ToListAsync();

                var campaignMembers = await UnitOfWork.Repository<CampaignMember>().GetAll(
                          predicate: x => x.CampaignId == request.CampaignId
                              && x.Status == (int)CampaignMemberStatus.Completed,
                          include: x => x.Include(y => y.Influencer),
                          selector: x => new
                          {
                              x.Id,
                              Influencer = x.Influencer,
                              Status = x.Status,
                              QuantityVoucherGet = x.VoucherCodes.Sum(y => y.QuantityGet),
                              QuantityVoucherUsed = x.VoucherCodes.Sum(y => y.QuantityUsed),
                              //TotalTransaction = x.VoucherCodes.Sum(y => y.VoucherTransactions.Count),
                              //TotalOrderAmount = x.VoucherCodes.Sum(y => y.VoucherTransactions.Sum(z => z.TotalOrderAmount)),
                              //TotalProductAmount = x.VoucherCodes.Sum(y => y.VoucherTransactions.Sum(z => z.TotalProductAmount)),
                              TotalVoucherCode = x.VoucherCodes.Count,
                              //TotalEarningAmount = x.VoucherCodes.Sum(y => y.VoucherTransactions.Sum(z => z.EarningMoney)),
                              TotalTransaction = 0,
                              TotalOrderAmount = new decimal(0),
                              TotalProductAmount = new decimal(0),
                              TotalEarningAmount = new decimal(0)
                          })
                      .ToListAsync();

                decimal maxTotalProductAmount = campaignMembers.Count == 0 ? 0 : campaignMembers.Max(x => x.TotalProductAmount);
                int rank = 1;
                report.CampaignMembers = campaignMembers.Select(x => new CampaignMemberReportViewModel
                {
                    Influencer = Mapper.Map<UserBasicViewModel>(x.Influencer),
                    Status = x.Status,
                    QuantityVoucherGet = x.QuantityVoucherGet,
                    QuantityVoucherUsed = x.QuantityVoucherUsed,
                    IsBestInfluencer = (x.TotalProductAmount == maxTotalProductAmount) && maxTotalProductAmount > 0,
                    TotalVoucherCode = x.TotalVoucherCode,
                    TotalEarningAmount = transactions.Where(t => t.VoucherCode.CampaignMemberId == x.Id).Sum(t => t.EarningMoney),
                    TotalTransaction = transactions.Where(t => t.VoucherCode.CampaignMemberId == x.Id).Count(),
                    TotalOrderAmount = transactions.Where(t => t.VoucherCode.CampaignMemberId == x.Id).Sum(t => t.TotalOrderAmount),
                    TotalProductAmount = transactions.Where(t => t.VoucherCode.CampaignMemberId == x.Id).Sum(t => t.TotalProductAmount),
                }).OrderByDescending(x => x.TotalProductAmount).Select(x =>
                {
                    x.Rank = rank++;
                    return x;
                }).ToList();

                report.NumberOfInfluencer = influencers.Count;
                report.NumberOfInfluencerCompleted = influencers.Where(x => x.Status == (int)CampaignMemberStatus.Completed).Count();
                report.NumberOfVoucher = vouchers.Count;
                report.NumberOfVoucherCode = voucherCodes.Count;
                report.TotalNumberVoucherCodeQuantity = voucherCodes.Sum(x => x.Quantity);
                report.TotalNumberVoucherCodeUsed = voucherCodes.Sum(x => x.QuantityUsed);
                report.TotalNumberVoucherCodeGet = campaignMembers.Sum(x => x.QuantityVoucherGet);

                report.TotalOrderAmount = report.CampaignMembers.Sum(x => x.TotalOrderAmount);
                report.TotalProductAmount = report.CampaignMembers.Sum(x => x.TotalProductAmount);
                report.TotalEarningMoney = report.CampaignMembers.Sum(x => x.TotalEarningAmount);
                report.TotalTransaction = report.CampaignMembers.Sum(x => x.TotalTransaction);

                return new Response<CampaignReportViewModel>(report);

            }
        }
    }
}
