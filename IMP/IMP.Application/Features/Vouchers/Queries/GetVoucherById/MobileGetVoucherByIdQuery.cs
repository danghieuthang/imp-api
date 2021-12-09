using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Queries.GetVoucherById
{
    public class MobileGetVoucherByIdQuery : IGetByIdQuery<Voucher, VoucherViewModel>
    {
        public int Id { get; set; }
        public class MobileGetVoucherByIdQueryHandler : GetByIdQueryHandler<MobileGetVoucherByIdQuery, Voucher, VoucherViewModel>
        {
            public MobileGetVoucherByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<VoucherViewModel>> Handle(MobileGetVoucherByIdQuery request, CancellationToken cancellationToken)
            {
                Func<IQueryable<Voucher>, IIncludableQueryable<Voucher, object>> include;

                include = vouchers => vouchers
                    .Include(voucher => voucher.CampaignVouchers).
                        ThenInclude(cv => cv.Campaign)
                            .ThenInclude(y => y.CampaignImages).Include(voucher => voucher.VoucherCodes)
                    .Include(voucher => voucher.CampaignVouchers)
                        .ThenInclude(cv => cv.Campaign)
                            .ThenInclude(c => c.Brand);

                var voucher = await Repository.FindSingleAsync(
                        predicate: x => x.Id == request.Id,
                        include: include);
                if (voucher == null)
                {
                    throw new KeyNotFoundException();
                }
                var voucherView = Mapper.Map<VoucherViewModel>(voucher);
                return new Response<VoucherViewModel>(voucherView);

            }
        }
    }
}
