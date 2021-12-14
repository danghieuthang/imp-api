using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace IMP.Application.Features.Campaigns.Queries.Reports
{
    public class ReportOfCampaignByDateQuery : IQuery<IEnumerable<CampaignReportByDateViewModel>>
    {
        [FromRoute(Name = "id")]
        public int CampaignId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime ToDate { get; set; }

        public class ReportOfCampaignByDateQueryValidator : AbstractValidator<ReportOfCampaignByDateQuery>
        {
            public ReportOfCampaignByDateQueryValidator()
            {
                RuleFor(x => x.FromDate).Must((x, y) => y <= x.ToDate).WithMessage("Ngày không hợp lệ");
            }
        }
        public class ReportOfCampaignByDateQueryHandler : QueryHandler<ReportOfCampaignByDateQuery, IEnumerable<CampaignReportByDateViewModel>>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public ReportOfCampaignByDateQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IEnumerable<CampaignReportByDateViewModel>>> Handle(ReportOfCampaignByDateQuery request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.Vouchers, x => x.CampaignMembers);

                if (campaign == null)
                {
                    throw new KeyNotFoundException();
                }

                if (campaign.BrandId != _authenticatedUserService.BrandId)
                {
                    throw new IMP.Application.Exceptions.ValidationException(new ValidationError("campaign_id", "Không có quyền."));
                }

                var campaignMemberIds = campaign.CampaignMembers.Select(x => x.Id).ToList();

                List<int> voucherIds = campaign.Vouchers.Select(x => x.VoucherId).ToList();

                var query = UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => x.CampaignMemberId.HasValue && campaignMemberIds.Contains(x.CampaignMemberId.Value),
                        include: x => x.Include(y => y.VoucherTransactions.Where(x =>
                                x.Created >= request.FromDate && x.Created <= request.ToDate)));

                var transactions = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => voucherIds.Contains(x.VoucherId),
                        include: x => x.Include(y => y.VoucherTransactions.Where(x =>
                                x.Created >= request.FromDate && x.Created <= request.ToDate)))
                    .SelectMany(x => x.VoucherTransactions)
                    .ToListAsync();

                var campaignReports = transactions.GroupBy(x => x.Created.Date).Select(g => new CampaignReportByDateViewModel
                {
                    Date = g.Key.Date,
                    TotalEarningMoney = g.Sum(x => x.EarningMoney),
                    TotalNumberVoucherCodeUsed = g.Count(),
                    TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                    TotalProductAmount = g.Sum(x => x.TotalProductAmount),
                    TotalTransaction = g.Count(),
                }).ToList();

                for (var date = request.FromDate; date <= request.ToDate; date = date.AddDays(1))
                {
                    if (!campaignReports.Any(x => x.Date == date.Date))
                    {
                        campaignReports.Add(new CampaignReportByDateViewModel
                        {
                            Date = date.Date.ToUniversalTime(),
                        });
                    }
                }

                return new Response<IEnumerable<CampaignReportByDateViewModel>>(campaignReports.OrderBy(x => x.Date));
            }
        }
    }
}
