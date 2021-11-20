using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Enums;
using IMP.Application.Validations;

namespace IMP.Application.Features.Vouchers.Queries.GetAllVoucher
{
    public class GetAllVoucherQuery : PageRequest, IListQuery<VoucherViewModel>
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "campaign_id")]
        public int? CampaignId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }
        public class GetAllVoucherQueryValidator : PageRequestValidator<GetAllVoucherQuery, VoucherViewModel>
        {
            public GetAllVoucherQueryValidator()
            {

            }
        }
        public class GetAllVoucherQueryHandler : ListQueryHandler<GetAllVoucherQuery, VoucherViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetAllVoucherQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<VoucherViewModel>>> Handle(GetAllVoucherQuery request, CancellationToken cancellationToken)
            {
                string name = string.IsNullOrEmpty(request.Name)?"" : request.Name.ToLower().Trim();
                var page = await UnitOfWork.Repository<Voucher>().GetPagedList(
                   predicate: x => x.BrandId == _authenticatedUserService.BrandId
                        && (request.FromDate == null || (request.FromDate != null && x.FromDate >= request.FromDate.Value))
                        && (request.ToDate == null || (request.ToDate != null && x.FromDate >= request.ToDate.Value))
                        && (request.CampaignId == null || (request.CampaignId != null && x.CampaignVouchers.Any(y => y.CampaignId == request.CampaignId.Value)))
                        && (string.IsNullOrEmpty(name) || x.VoucherName.ToLower().Contains(name)),
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
