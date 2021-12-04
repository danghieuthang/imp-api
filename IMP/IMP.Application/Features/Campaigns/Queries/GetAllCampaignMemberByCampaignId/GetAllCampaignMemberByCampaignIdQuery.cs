using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Queries.GetAllCampaignMemberByCampaignId
{
    public class GetAllCampaignMemberByCampaignIdQuery : PageRequest, IListQuery<CampaignMemberViewModel>
    {
        private int _campaignId;
        [FromQuery(Name = "status")]
        public CampaignMemberStatus? Status { get; set; }
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        [FromQuery(Name = "is_get_member_activities")]
        public bool IsGetMemberActivities { get; set; }

        public class GetAllCampaignMemberCampaignIdQueryHandler : ListQueryHandler<GetAllCampaignMemberByCampaignIdQuery, CampaignMemberViewModel>
        {
            public GetAllCampaignMemberCampaignIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IPagedList<CampaignMemberViewModel>>> Handle(GetAllCampaignMemberByCampaignIdQuery request, CancellationToken cancellationToken)
            {
                Func<IQueryable<CampaignMember>, IIncludableQueryable<CampaignMember, object>> include;
                if (request.IsGetMemberActivities)
                {
                    include = campaignMembers => campaignMembers.Include(y => y.Influencer).Include(y => y.ApprovedBy).Include(x => x.MemberActivities);
                }
                else
                {
                    include = campaignMembers => campaignMembers.Include(y => y.Influencer).Include(y => y.ApprovedBy);
                }

                var page = await UnitOfWork.Repository<CampaignMember>().GetPagedList(
                    predicate: x => x.CampaignId == request.Id
                        && (request.Status == null || (request.Status != null && (int)request.Status == x.Status)),
                    include: include,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    orderBy: request.OrderField,
                    orderByDecensing: request.OrderBy == OrderBy.DESC,
                    cancellationToken: cancellationToken);

                var view = page.ToResponsePagedList<CampaignMemberViewModel>(Mapper);
                return new Response<IPagedList<CampaignMemberViewModel>>(view);
            }
        }
    }
}
