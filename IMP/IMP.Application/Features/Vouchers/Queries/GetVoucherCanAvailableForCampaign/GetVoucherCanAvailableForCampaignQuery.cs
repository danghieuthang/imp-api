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

namespace IMP.Application.Features.Vouchers.Queries.GetVoucherCanAvailableForCampaign
{
    public class GetVoucherCanAvailableForCampaignQuery : PageRequest, IListQuery<VoucherViewModel>
    {
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }
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

                var page = await UnitOfWork.Repository<Voucher>().GetPagedList(
                 predicate: x => (x.ToDate == null || (x.ToDate != null && x.ToDate.Value.CompareTo(campaign.AdvertisingDate.Value) > 0))
                    && (x.QuantityUsed < x.Quantity)
                    && x.BrandId == _authenticatedUserService.BrandId,
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
