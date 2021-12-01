using AutoMapper;
using IMP.Application.Enums;
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

namespace IMP.Application.Features.Vouchers.Queries.GetAllVoucherByApplicationUser
{
    public class GetAllVoucherByInfluencerQuery : PageRequest, IListQuery<UserVoucherViewModel>
    {
        //[FromQuery(Name = "from_date")]
        //public DateTime? FromDate { get; set; }
        //[FromQuery(Name = "to_date")]
        //public DateTime? ToDate { get; set; }
        [FromQuery(Name = "campaign_id")]
        public int? CampaignId { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "is_expired")]
        public bool? IsExpired { get; set; }
        [FromQuery(Name = "discount_value_type")]
        public DiscountValueType? DiscountValueType { get; set; }

    }
    public class GetAllVoucherByInfluencerQueryValidator : PageRequestValidator<GetAllVoucherByInfluencerQuery, UserVoucherViewModel>
    {
        public GetAllVoucherByInfluencerQueryValidator()
        {
        }
    }

    public class GetAllVoucherByInfluencerQueryHandler : ListQueryHandler<GetAllVoucherByInfluencerQuery, UserVoucherViewModel>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public GetAllVoucherByInfluencerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public override async Task<Response<IPagedList<UserVoucherViewModel>>> Handle(GetAllVoucherByInfluencerQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                request.Name = "";
            }

            var page = await UnitOfWork.Repository<Voucher>().GetPagedList(
                    predicate: x => x.VoucherCodes.Any(y => y.CampaignMember.InfluencerId == _authenticatedUserService.ApplicationUserId)
                        && (request.DiscountValueType == null
                            || (request.DiscountValueType.HasValue
                                && x.DiscountValueType == (int)request.DiscountValueType))
                        && (x.VoucherName.ToLower().Contains(request.Name))
                        && (request.IsExpired == null
                            || (request.IsExpired.HasValue
                                && request.IsExpired.Value == true
                                && x.ToDate >= DateTime.UtcNow.Date)
                            || (request.IsExpired.HasValue
                                && request.IsExpired.Value == false
                                && (x.ToDate == null || x.ToDate < DateTime.UtcNow.Date)))
                        && (request.CampaignId == null
                            || (request.CampaignId.HasValue
                                && x.CampaignVouchers.Any(y => y.CampaignId == request.CampaignId.Value))),
                    include: x => x.Include(y => y.VoucherCodes.Where(z => z.CampaignMember.InfluencerId == _authenticatedUserService.ApplicationUserId)),
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    orderBy: request.OrderField,
                    orderByDecensing: request.OrderBy == OrderBy.DESC,
                    cancellationToken: cancellationToken);

            var pageResponse = page.ToResponsePagedList<UserVoucherViewModel>(Mapper);
            return new Response<IPagedList<UserVoucherViewModel>>(pageResponse);

        }
    }
}
