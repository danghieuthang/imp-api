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

namespace IMP.Application.Features.VoucherCodes.Queries.GetAllVoucherCodeOfCampaignMember
{
    public class GetAllVoucherCodeOfCampaignMemberQuery : IQuery<IEnumerable<VoucherCodeViewModel>>
    {
        public int CampaignId { get; set; }
        public class GetAllVoucherCodeOfCampaignMemberQueryHandler : QueryHandler<GetAllVoucherCodeOfCampaignMemberQuery, IEnumerable<VoucherCodeViewModel>>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetAllVoucherCodeOfCampaignMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IEnumerable<VoucherCodeViewModel>>> Handle(GetAllVoucherCodeOfCampaignMemberQuery request, CancellationToken cancellationToken)
            {
                var voucherCodes = await UnitOfWork.Repository<VoucherCode>().GetAll(predicate: x => x.CampaignMember.CampaignId == request.CampaignId && x.CampaignMember.InfluencerId == _authenticatedUserService.ApplicationUserId).ToListAsync();
                var voucherCodeViews = Mapper.Map<IEnumerable<VoucherCodeViewModel>>(voucherCodes);
                return new Response<IEnumerable<VoucherCodeViewModel>>(data: voucherCodeViews);
            }
        }
    }
}
