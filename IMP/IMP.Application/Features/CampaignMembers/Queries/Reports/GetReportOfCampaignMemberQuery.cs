using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignMembers.Queries.Reports
{
    public class GetReportOfCampaignMemberQuery : IQuery<CampaignMemberReportViewModel>
    {
        public int Id { get; set; }
        public class GetReportOfCampaignMemberQueryHandler : QueryHandler<GetReportOfCampaignMemberQuery, CampaignMemberReportViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetReportOfCampaignMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CampaignMemberReportViewModel>> Handle(GetReportOfCampaignMemberQuery request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(
                        predicate: x => x.Id == request.Id,
                        include: x => x.Include(y => y.VoucherCodes).ThenInclude(z => z.VoucherTransactions)
                            .Include(y => y.VoucherCodes).ThenInclude(y => y.Voucher));
                if (campaignMember == null)
                {
                    throw new KeyNotFoundException();
                }
                var report = new CampaignMemberReportViewModel
                {
                    QuantityVoucherGet = campaignMember.VoucherCodes.Sum(x => x.QuantityGet),
                    QuantityVoucherUsed = campaignMember.VoucherCodes.Sum(x => x.QuantityUsed),
                    Status = campaignMember.Status,
                };
                return new Response<CampaignMemberReportViewModel>(report);
            }
        }
    }
}
