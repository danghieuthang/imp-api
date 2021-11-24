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

namespace IMP.Application.Features.CampaignMembers.Queries.GetCampaignMemberOfCampaignByInfluencerId
{
    public class GetCampaignMemberOfCampaignByInfluencerIdQuery : IQuery<CampaignMemberViewModel>
    {
        public int CampaignId { get; set; }
        public class GetCampaignMemberOfCampaignByInfluencerIdQueryHandler : QueryHandler<GetCampaignMemberOfCampaignByInfluencerIdQuery, CampaignMemberViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetCampaignMemberOfCampaignByInfluencerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CampaignMemberViewModel>> Handle(GetCampaignMemberOfCampaignByInfluencerIdQuery request, CancellationToken cancellationToken)
            {
                var campaignMember = await UnitOfWork.Repository<CampaignMember>().FindSingleAsync(
                        predicate: x => x.CampaignId == request.CampaignId && x.InfluencerId == _authenticatedUserService.ApplicationUserId,
                        include: x => x.Include(y => y.MemberActivities).ThenInclude(z => z.CampaignActivity));
                if (campaignMember == null)
                {
                    throw new KeyNotFoundException();
                }
                var campaignMemberView = Mapper.Map<CampaignMemberViewModel>(campaignMember);
                return new Response<CampaignMemberViewModel>(campaignMemberView);
            }
        }
    }
}
