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

namespace IMP.Application.Features.VoucherTransactions.Queries.GetTransactionReportOfCampaign
{
    public class GetTransactionReportOfCampaignQuery : IQuery<VoucherTransactionReportViewModel>
    {
        public int CampaignId { get; set; }
        public class GetTransactionReportOfCampaignQueryHandler : QueryHandler<GetTransactionReportOfCampaignQuery, VoucherTransactionReportViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetTransactionReportOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<VoucherTransactionReportViewModel>> Handle(GetTransactionReportOfCampaignQuery request, CancellationToken cancellationToken)
            {
                var voucherIds = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                    predicate: x => x.CampaignId == request.CampaignId,
                    selector: x => x.VoucherId
                    ).ToListAsync();
                if (!voucherIds.Any())
                {
                    return new Response<VoucherTransactionReportViewModel>();
                }

                var voucherTransactions = await UnitOfWork.Repository<VoucherTransaction>().GetAll(
                    predicate: x => voucherIds.Contains(x.VoucherCode.VoucherId)
                    ).ToListAsync();
                var report = new VoucherTransactionReportViewModel
                {
                    TotalCode = voucherTransactions.GroupBy(x => x.VoucherCodeId).Count(),
                    TotalTransaction = voucherTransactions.Count(),
                    TotalOrderPrice = voucherTransactions.Sum(x => x.TotalPrice),
                    TotalDiscount = voucherTransactions.Sum((x) => x.TotalDiscount),
                    TotalOrderAmount = voucherTransactions.GroupBy(x => x.OrderCode).Count()
                };
                return new Response<VoucherTransactionReportViewModel>(report);
            }
        }
    }
}
