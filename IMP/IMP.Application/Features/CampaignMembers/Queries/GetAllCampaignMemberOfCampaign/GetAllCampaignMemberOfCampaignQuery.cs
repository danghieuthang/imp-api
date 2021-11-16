using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignMembers.Queries.GetAllCampaignMemberOfCampaign
{
    public class GetAllCampaignMemberOfCampaignQuery : PageRequest, IListQuery<CampaignMemberViewModel>
    {
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }
        [FromQuery(Name = "status")]
        public CampaignMemberStatus? Status { get; set; }

        public class GetAllCampaignMemberOfCampaignQueryHandler : ListQueryHandler<GetAllCampaignMemberOfCampaignQuery, CampaignMemberViewModel>
        {
            public GetAllCampaignMemberOfCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IPagedList<CampaignMemberViewModel>>> Handle(GetAllCampaignMemberOfCampaignQuery request, CancellationToken cancellationToken)
            {
                var page = await UnitOfWork.Repository<CampaignMember>().GetPagedList(
                 predicate: x => x.CampaignId == request.CampaignId
                     && (request.Status == null || (request.Status != null && (int)request.Status == x.Status)),
                     include: x => x.Include(y => y.Influencer).Include(y => y.MemberActivities).ThenInclude(z => z.Evidence),
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
