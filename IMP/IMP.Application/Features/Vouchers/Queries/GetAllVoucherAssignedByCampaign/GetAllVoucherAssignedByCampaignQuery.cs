using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Vouchers.Queries.GetAllVoucherAssignedByCampaign
{
    public class GetAllVoucherAssignedByCampaignQuery : IQuery<IEnumerable<VoucherViewModel>>
    {
        public int CampaignId { get; set; }

        public class GetAllVoucherAssignedByCampaignQueryHandler : QueryHandler<GetAllVoucherAssignedByCampaignQuery, IEnumerable<VoucherViewModel>>
        {
            public GetAllVoucherAssignedByCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetAllVoucherAssignedByCampaignQuery request, CancellationToken cancellationToken)
            {
                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                        predicate: x => x.CampaignId == request.CampaignId,
                        selector: x => x.Voucher,
                        include: x => x.Include(y => y.Voucher).ThenInclude(z => z.VoucherCodes)).ToListAsync();

                vouchers = vouchers.Where(x => x.VoucherCodes.Any(y => y.CampaignMemberId.HasValue)).ToList();
                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);
            }
        }
    }
}
