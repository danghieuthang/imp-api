using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using IMP.Application.Validations;
using Microsoft.EntityFrameworkCore;

namespace IMP.Application.Features.Vouchers.Queries.GetVoucherCanAvailableForCampaign
{
    public class GetVoucherCanAvailableForCampaignQuery : PageRequest, IListQuery<VoucherViewModel>
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }

        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }

        [FromQuery(Name = "include_existed_in_campaign")]
        public bool IncludeExisedInCampaign { get; set; }
        public class GetVoucherCanAvailableForCampaignQueryValidator : PageRequestValidator<GetVoucherCanAvailableForCampaignQuery, VoucherViewModel>
        {
            public GetVoucherCanAvailableForCampaignQueryValidator()
            {

            }
        }
        public class GetVoucherCanAvailableForCampaignQueryHandler : ListQueryHandler<GetVoucherCanAvailableForCampaignQuery, VoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetVoucherCanAvailableForCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<VoucherViewModel>>> Handle(GetVoucherCanAvailableForCampaignQuery request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
                if (campaign == null)
                {
                    return new Response<IPagedList<VoucherViewModel>>(new PagedList<VoucherViewModel>());
                }

                string name = string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToLower().Trim();


                var page = await UnitOfWork.Repository<Voucher>().GetPagedList(
                 predicate: x => (x.ToDate == null
                        || (x.ToDate != null && x.ToDate.Value.CompareTo(DateTime.Now.Date) >= 0))
                    && (x.QuantityUsed < x.Quantity)
                    && x.BrandId == _authenticatedUserService.BrandId
                    && (request.FromDate == null || (request.FromDate != null && x.FromDate >= request.FromDate.Value))
                    && (request.ToDate == null || (request.ToDate != null && x.FromDate >= request.ToDate.Value))
                    && (string.IsNullOrEmpty(name) || x.VoucherName.ToLower().Contains(name))
                    && (request.IncludeExisedInCampaign == true
                        || (request.IncludeExisedInCampaign == false && !x.CampaignVouchers.Any(y => y.CampaignId == request.CampaignId && y.IsBestInfluencerReward == false && y.IsDefaultReward == false))),

                 include: x => x.Include(y => y.CampaignVouchers).Include(z => z.VoucherCodes),
                 pageIndex: request.PageIndex,
                 pageSize: request.PageSize,
                 orderBy: request.OrderField,
                 orderByDecensing: request.OrderBy == OrderBy.DESC);

                var pageView = page.ToResponsePagedList<VoucherViewModel>(Mapper);
                return new Response<IPagedList<VoucherViewModel>>(pageView);
            }
        }
    }
}
