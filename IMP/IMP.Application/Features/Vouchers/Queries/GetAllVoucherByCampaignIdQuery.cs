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

namespace IMP.Application.Features.Vouchers.Queries
{
    public class GetAllVoucherByCampaignIdQuery : IGetAllQuery<VoucherViewModel>
    {
        public int CampaignId { get; set; }
        public class GetAllVoucherByCampaignIdQueryHandler : GetAllQueryHandler<GetAllVoucherByCampaignIdQuery, CampaignVoucher, VoucherViewModel>
        {
            public GetAllVoucherByCampaignIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetAllVoucherByCampaignIdQuery request, CancellationToken cancellationToken)
            {
                List<Voucher> vouchers = await Repository.GetAll(
                        selector: x => x.Voucher,
                        predicate: x => x.CampaignId == request.CampaignId,
                        include: x => x.Include(y => y.Voucher)).ToListAsync();
                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);
            }
        }
    }
}