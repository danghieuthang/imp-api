﻿using AutoMapper;
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
                        && (request.FromDate == null || (request.FromDate.HasValue && x.Created >= request.FromDate.Value))
                        && (request.ToDate == null || (request.ToDate.HasValue && x.Created <= request.ToDate)))
                        .ToListAsync();
                var reports = transactions.GroupBy(x => x.Created.Date).Select(g => new ReportVoucherTransactionOfMemberViewModel
                {
                    Date = g.Key,
                    TotalMoneyEarning = g.Sum(x => x.EarningMoney),
                    TotalVoucherTransaction = g.Count(),
                    TotalProductAmount = g.Sum(x => x.TotalProductAmount),
                    TotalProductQuantity = g.Sum(x => x.ProductQuantity)
                });
                return new Response<IEnumerable<ReportVoucherTransactionOfMemberViewModel>>(reports);

            }
        }
    }
}
