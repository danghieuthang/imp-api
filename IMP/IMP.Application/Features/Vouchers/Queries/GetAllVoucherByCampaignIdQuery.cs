using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Vouchers.Queries
{
    public class GetAllVoucherByCampaignIdQuery : IGetAllQuery<VoucherViewModel>
    {
        public int CampaignId { get; set; }
        public class GetAllVoucherByCampaignIdQueryHandler : GetAllQueryHandler<GetAllVoucherByCampaignIdQuery, Voucher, VoucherViewModel>
        {
            public GetAllVoucherByCampaignIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetAllVoucherByCampaignIdQuery request, CancellationToken cancellationToken)
            {
                var vouchers = await Repository.FindAllAsync(x => x.CampaignId == request.CampaignId);
                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);
            }
        }
    }
}