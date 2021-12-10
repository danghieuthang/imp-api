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

namespace IMP.Application.Features.Campaigns.Queries.Reports
{
    public class GetReportVoucherOfCampaignQuery : IQuery<IEnumerable<VoucherTransactionReportOfVoucherViewModel>>
    {
        public int CampaignId { get; set; }
        public class GetReportVoucherOfCampaignQueryHandler : QueryHandler<GetReportVoucherOfCampaignQuery, IEnumerable<VoucherTransactionReportOfVoucherViewModel>>
        {
            public GetReportVoucherOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherTransactionReportOfVoucherViewModel>>> Handle(GetReportVoucherOfCampaignQuery request, CancellationToken cancellationToken)
            {

                var influencers = await UnitOfWork.Repository<CampaignMember>().GetAll(
                       predicate: x => x.CampaignId == request.CampaignId,
                       selector: x => x.Id).ToListAsync();


                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => x.Voucher,
                        include: x =>
                            x.Include(y => y.Voucher)
                                .ThenInclude(z => z.VoucherCodes.Where(vc => vc.CampaignMemberId.HasValue && influencers.Contains(vc.CampaignMemberId.Value)))
                                    .ThenInclude(z => z.VoucherTransactions)).ToListAsync();


                var report = vouchers.Select(x => new VoucherTransactionReportOfVoucherViewModel
                {
                    VoucherId = x.Id,
                    Name = x.VoucherName,
                    TotalTransaction = vouchers.Sum(x => x.VoucherCodes.Sum(y => y.VoucherTransactions.Count)),
                    TotalVoucherCode = vouchers.Sum(x => x.VoucherCodes.Count),
                    TotalOrderAmount = vouchers.Sum(x => x.VoucherCodes.Count),
                    QuantityGet = vouchers.Sum(x => x.VoucherCodes.Sum(y => y.QuantityGet)),
                    QuantityUsed = vouchers.Sum(x => x.VoucherCodes.Sum(y => y.QuantityUsed)),
                    TotalVoucherCodeQuantity = vouchers.Sum(x => x.VoucherCodes.Sum(y => y.Quantity)),
                    TotalEarningAmount = vouchers.Sum(x => x.VoucherCodes.Sum(y => y.VoucherTransactions.Sum(z => z.EarningMoney))),
                    TotalProductAmount = vouchers.Sum(x => x.VoucherCodes.Sum(y => y.VoucherTransactions.Sum(z => z.TotalProductAmount))),
                });

                return new Response<IEnumerable<VoucherTransactionReportOfVoucherViewModel>>(report);
            }
        }
    }
}
