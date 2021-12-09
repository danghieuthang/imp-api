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

namespace IMP.Application.Features.VoucherTransactions.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : IQuery<VoucherTransactionViewModel>
    {
        public int Id { get; set; }
        public class GetTransactionByIdQueryHandler : QueryHandler<GetTransactionByIdQuery, VoucherTransactionViewModel>
        {
            public GetTransactionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<VoucherTransactionViewModel>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
            {
                var transaction = await UnitOfWork.Repository<VoucherTransaction>().FindSingleAsync(
                       predicate: x => x.Id == request.Id,
                       include: x => x.Include(y => y.VoucherCode).ThenInclude(z => z.Voucher)
                        .Include(y => y.VoucherCode).ThenInclude(y => y.CampaignMember)
                       );
                var influencer = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == transaction.VoucherCode.CampaignMember.InfluencerId);

                var view = Mapper.Map<VoucherTransactionViewModel>(transaction);
                view.Influencer = Mapper.Map<InfluencerViewModel>(influencer);
                return new Response<VoucherTransactionViewModel>(view);
            }
        }
    }
}
