using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Queries.GetRangeOfVoucher
{
    public class GetRangeOfVoucherQuery : IQuery<RangeVoucherDateViewModel>
    {
        public class GetRangeOfVoucherQueryHandler : QueryHandler<GetRangeOfVoucherQuery, RangeVoucherDateViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetRangeOfVoucherQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<RangeVoucherDateViewModel>> Handle(GetRangeOfVoucherQuery request, CancellationToken cancellationToken)
            {
                var minDate = UnitOfWork.Repository<Voucher>().GetAll(
                        predicate: x => x.BrandId == _authenticatedUserService.BrandId,
                        selector: x => x.FromDate,
                        orderBy: x => x.OrderBy(y => y.FromDate))
                    .FirstOrDefault();

                var maxDate = UnitOfWork.Repository<Voucher>().GetAll(
                        predicate: x => x.BrandId == _authenticatedUserService.BrandId,
                        selector: x => x.ToDate,
                        orderBy: x => x.OrderByDescending(y => y.ToDate))
                    .FirstOrDefault();

                return new Response<RangeVoucherDateViewModel>(new RangeVoucherDateViewModel { MaxDate = maxDate, MinDate = minDate });

            }
        }
    }
}
