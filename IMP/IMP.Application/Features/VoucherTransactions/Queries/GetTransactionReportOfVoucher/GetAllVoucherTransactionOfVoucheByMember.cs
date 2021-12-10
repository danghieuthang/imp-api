using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Validations;
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
    public class GetAllVoucherTransactionOfVoucherQuery : PageRequest, IListQuery<VoucherTransactionViewModel>
    {
        [FromQuery(Name = "campaign_member_id")]
        public int CampaignMemberId { get; set; }
        [FromQuery(Name = "voucher_id")]
        public int VoucherId { get; set; }
        public class GetAllVoucherTransactionOfVoucherQueryValidator : PageRequestValidator<GetAllVoucherTransactionOfVoucherQuery, VoucherTransactionViewModel>
        {

        }
        public class GetAllVoucherTransactionOfVoucherQueryHandler : ListQueryHandler<GetAllVoucherTransactionOfVoucherQuery, VoucherTransactionViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetAllVoucherTransactionOfVoucherQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<VoucherTransactionViewModel>>> Handle(GetAllVoucherTransactionOfVoucherQuery request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(x => x.Id == request.CampaignMemberId,
                       include: x => x.Include(y => y.Campaign));

                if (campaignMember == null)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Chiến dịch không tồn tại."));
                }
                if (campaignMember.InfluencerId != _authenticatedUserService.ApplicationUserId)
                {
                    throw new ValidationException(new ValidationError("campaign_member_id", "Không có quyền."));
                }

                var page = await UnitOfWork.Repository<VoucherTransaction>().GetPagedList(
                        predicate: x => x.VoucherCode.CampaignMemberId == request.CampaignMemberId && x.VoucherCode.VoucherId == request.VoucherId,
                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == Enums.OrderBy.DESC,
                        pageIndex: request.PageIndex,
                        pageSize: request.PageSize,
                        cancellationToken: cancellationToken,
                        include: x => x.Include(y => y.VoucherCode)
                        );

                var pageView = page.ToResponsePagedList<VoucherTransactionViewModel>(Mapper);

                pageView.Items = pageView.Items.Select(x =>
                {
                    x.TotalOrderAmount = 0; // influencer can't see total order amount
                    return x;
                }).ToList();

                return new Response<IPagedList<VoucherTransactionViewModel>>(data: pageView);
            }
        }
    }
}
