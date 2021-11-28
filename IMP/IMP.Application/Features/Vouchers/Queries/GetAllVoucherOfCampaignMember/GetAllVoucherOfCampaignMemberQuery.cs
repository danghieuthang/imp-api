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

namespace IMP.Application.Features.Vouchers.Queries.GetAllVoucherOfCampaignMember
{
    public class GetAllVoucherOfCampaignMemberQuery : IQuery<IEnumerable<VoucherViewModel>>
    {
        public int CampaignMemberId { get; set; }
        public class GetAllVoucherOfCampaignMemberQueryHandler : QueryHandler<GetAllVoucherOfCampaignMemberQuery, IEnumerable<VoucherViewModel>>
        {
            public GetAllVoucherOfCampaignMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<VoucherViewModel>>> Handle(GetAllVoucherOfCampaignMemberQuery request, CancellationToken cancellationToken)
            {
                var voucherCodes = await UnitOfWork.Repository<VoucherCode>().GetAll(
                        predicate: x => x.CampaignMemberId == request.CampaignMemberId,
                        include: x => x.Include(y => y.Voucher)).ToListAsync();

                var vouchers = voucherCodes.GroupBy(x => x.VoucherId).Select(g =>
                {
                    var voucher = g.ToList()[0].Voucher;
                    voucher.VoucherCodes = g.ToList();
                    return voucher;
                }).ToList();

                var voucherViews = Mapper.Map<IEnumerable<VoucherViewModel>>(vouchers);
                return new Response<IEnumerable<VoucherViewModel>>(voucherViews);



            }
        }
    }
}
