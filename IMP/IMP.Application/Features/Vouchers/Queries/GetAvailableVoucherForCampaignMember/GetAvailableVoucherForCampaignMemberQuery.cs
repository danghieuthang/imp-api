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

namespace IMP.Application.Features.Vouchers.Queries.GetAvailableVoucherForCampaignMember
{
    public class GetAvailableVoucherForCampaignMemberQuery : IQuery<IEnumerable<VoucherViewModel>>
    {
        public int CampaignId { get; set; }
        public class GetAvailableVoucherForCampaignMemberQueryHandler : QueryHandler<GetAvailableVoucherForCampaignMemberQuery, IEnumerable<VoucherViewModel>>
        {
            public GetAvailableVoucherForCampaignMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetAvailableVoucherForCampaignMemberQuery request, CancellationToken cancellationToken)
            {
                var vouchers = await UnitOfWork.Repository<CampaignVoucher>().GetAll(
                    selector: x => x.Voucher,
                    predicate: x => x.CampaignId == request.CampaignId && x.IsDefaultReward == false && x.IsBestInfluencerReward == false
                        && x.Voucher.QuantityUsed < x.Voucher.Quantity,
                    include: x => x.Include(y => y.Voucher).ThenInclude(z => z.VoucherCodes)).ToListAsync();

                vouchers.ForEach(x =>
                {
                    x.VoucherCodes = x.VoucherCodes.Where(x => x.CampaignMemberId == null).ToList();
                });

                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);
            }
        }
    }
}
