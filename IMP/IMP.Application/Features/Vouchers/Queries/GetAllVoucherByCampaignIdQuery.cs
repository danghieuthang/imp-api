using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace IMP.Application.Features.Vouchers.Queries
{
    public class GetAllVoucherByCampaignIdQuery : IGetAllQuery<VoucherViewModel>
    {
        public int CampaignId { get; set; }
        public bool IsGetVoucherCodes { get; set; }
        public class GetAllVoucherByCampaignIdQueryHandler : GetAllQueryHandler<GetAllVoucherByCampaignIdQuery, CampaignVoucher, VoucherViewModel>
        {
            public GetAllVoucherByCampaignIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetAllVoucherByCampaignIdQuery request, CancellationToken cancellationToken)
            {
                Func<IQueryable<CampaignVoucher>, IIncludableQueryable<CampaignVoucher, object>> funcInclude;
                if (request.IsGetVoucherCodes)
                {
                    funcInclude = campaignVouchers => campaignVouchers.Include(y => y.Voucher).ThenInclude(y => y.VoucherCodes);
                }
                else
                {
                    funcInclude = campaignVouchers => campaignVouchers.Include(y => y.Voucher);
                }

                var campaignVouchers = await Repository.GetAll(
                         predicate: x => x.CampaignId == request.CampaignId && x.IsDefaultReward == false && x.IsBestInfluencerReward == false,
                         include: funcInclude).ToListAsync();

                var vouchers = campaignVouchers.GroupBy(x => x.Voucher).Select(x =>
                {
                    var voucher = x.Key;
                    voucher.CampaignVouchers = x.ToList();
                    return voucher;
                }).ToList();

                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);
            }
        }
    }
}