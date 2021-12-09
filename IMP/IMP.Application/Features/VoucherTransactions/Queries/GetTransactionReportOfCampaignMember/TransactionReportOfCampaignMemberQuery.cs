using AutoMapper;
using IMP.Application.Interfaces;
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

namespace IMP.Application.Features.VoucherTransactions.Queries.GetTransactionReportOfCampaignMember
{
    public class TransactionReportOfCampaignMemberQuery : IQuery<IEnumerable<ReportVoucherTransactionOfMemberViewModel>>
    {
        [FromRoute(Name = "id")]
        public int CampaignMemberId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }
        public class TransactionReportOfCampaignMemberQueryHandler : QueryHandler<TransactionReportOfCampaignMemberQuery, IEnumerable<ReportVoucherTransactionOfMemberViewModel>>
        {
            public TransactionReportOfCampaignMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<ReportVoucherTransactionOfMemberViewModel>>> Handle(TransactionReportOfCampaignMemberQuery request, CancellationToken cancellationToken)
            {

                var transactions = await UnitOfWork.Repository<VoucherTransaction>().GetAll(
                    predicate: x => x.VoucherCode.CampaignMemberId == request.CampaignMemberId
                        && (request.FromDate == null || (request.FromDate.HasValue && x.Created.Date.CompareTo(request.FromDate.Value) >= 0))
                        && (request.ToDate == null || (request.ToDate.HasValue && x.Created.Date.CompareTo(request.ToDate.Value) <= 0)))
                        .ToListAsync();

                var reports = transactions.GroupBy(x => x.Created.Date).Select(g => new ReportVoucherTransactionOfMemberViewModel
                {
                    Date = g.Key,
                    TotalMoneyEarning = g.Sum(x => x.EarningMoney),
                    TotalVoucherTransaction = g.Count(),
                    TotalProductAmount = g.Sum(x => x.TotalProductAmount),
                    TotalProductQuantity = g.Sum(x => x.ProductQuantity)
                }).ToList();
                if (request.FromDate != null && request.ToDate != null)
                {
                    for (var date = request.FromDate.Value; date <= request.ToDate.Value; date = date.AddDays(1))
                    {
                        if (!reports.Any(x => x.Date == date.Date))
                        {
                            reports.Add(new ReportVoucherTransactionOfMemberViewModel { Date = date.ToUniversalTime() });
                        }
                    }
                }
                return new Response<IEnumerable<ReportVoucherTransactionOfMemberViewModel>>(reports.OrderBy(x => x.Date));

            }
        }
    }
}
