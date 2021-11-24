using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IMP.Application.Features.MemberActivities.Queries.GetAllMemberActivitesOfUser
{
    public class GetAllMemberAcitivitiesOfUserQuery : IQuery<List<MemberActivityViewModel>>
    {
        public int? ApplicationUserId { get; set; }
        public int? CampaignMemberId { get; set; }
        public class GetAllMemberActivitiesOfUserQueryHandler : QueryHandler<GetAllMemberAcitivitiesOfUserQuery, List<MemberActivityViewModel>>
        {
            public GetAllMemberActivitiesOfUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<List<MemberActivityViewModel>>> Handle(GetAllMemberAcitivitiesOfUserQuery request, CancellationToken cancellationToken)
            {
                var memberActivities = UnitOfWork.Repository<MemberActivity>().GetAll(
                        predicate: x => (request.ApplicationUserId != null && request.ApplicationUserId.Value == x.CampaignMember.InfluencerId)
                            || (request.CampaignMemberId != null && request.CampaignMemberId.Value == x.CampaignMemberId),
                        include: x => x.Include(y => y.CampaignActivity).Include(z => z.Evidences)
                            .Include(y => y.ActivityComments).ThenInclude(y => y.ApplicationUser)
                            ).ToList();

                var memberActivityViews = Mapper.Map<List<MemberActivityViewModel>>(memberActivities);
                return new Response<List<MemberActivityViewModel>>(memberActivityViews);
            }
        }
    }
}
