using AutoMapper;
using FluentValidation;
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
    public class GetAllVoucherByCustomerQuery : PageRequest, IListQuery<UserVoucherViewModel>
    {
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }

    }
    public class GetAllVoucherByApplicationUserQueryValidator : PageRequestValidator<GetAllVoucherByCustomerQuery, UserVoucherViewModel>
    {
        public GetAllVoucherByApplicationUserQueryValidator()
        {

        }
    }
    public class GetAllVoucherByApplicationUserQueryHandler : ListQueryHandler<GetAllVoucherByCustomerQuery, UserVoucherViewModel>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public GetAllVoucherByApplicationUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        public override async Task<Response<IPagedList<UserVoucherViewModel>>> Handle(GetAllVoucherByCustomerQuery request, CancellationToken cancellationToken)
        {


            var page = await UnitOfWork.Repository<VoucherCodeApplicationUser>().GetPagedList(
                    predicate: x => x.ApplicationUserId == _authenticatedUserService.ApplicationUserId,
                    include: x => x.Include(y => y.VoucherCode).ThenInclude(z => z.Voucher),
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    orderBy: request.OrderField,
                    orderByDecensing: request.OrderBy == OrderBy.DESC);

            var items = page.Items;

            var vouchers = page.Items.GroupBy(x => x.VoucherCode.VoucherId).Select(g =>
            {
                var a = items.Where(i => i.VoucherCode.VoucherId == g.Key).Select(x => x.VoucherCode.Voucher).FirstOrDefault();
                a.VoucherCodes = g.Select(x => x.VoucherCode).ToList();
                return a;
            }).ToList();

            //var voucherCodes = page.Items.Select(x => x.VoucherCode).ToList();
            var voucherCodesView = Mapper.Map<List<UserVoucherViewModel>>(vouchers);
            var pageResponse = new PagedList<UserVoucherViewModel>
            {
                Items = voucherCodesView,
                PageIndex = page.PageIndex,
                PageSize = page.PageSize,
                TotalCount = page.TotalCount,
                TotalPages = page.TotalPages
            };
            return new Response<IPagedList<UserVoucherViewModel>>(pageResponse);

        }
    }
}
