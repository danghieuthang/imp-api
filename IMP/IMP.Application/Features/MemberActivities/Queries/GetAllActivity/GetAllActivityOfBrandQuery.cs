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

namespace IMP.Application.Features.MemberActivities.Queries.GetAllActivity
{
    public class GetAllActivityOfBrandQuery : PageRequest, IListQuery<MemberActivityViewModel>
    {
        public GetAllActivityOfBrandQuery()
        {
            Status = new List<MemberActivityStatus>();
            CampaignIds = new List<int>();
        }
        [FromQuery(Name = "status")]
        public List<MemberActivityStatus> Status { get; set; }
        [FromQuery(Name = "campaign_id")]
        public List<int> CampaignIds { get; set; }

        public class GetAllActivityOfBrandQueryHandler : ListQueryHandler<GetAllActivityOfBrandQuery, MemberActivityViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetAllActivityOfBrandQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<MemberActivityViewModel>>> Handle(GetAllActivityOfBrandQuery request, CancellationToken cancellationToken)
            {
                List<int> status = request.Status.Select(x => (int)x).ToList();
                var page = await UnitOfWork.Repository<MemberActivity>().GetPagedList(
                  predicate: x => x.CampaignMember.Campaign.BrandId == _authenticatedUserService.BrandId
                        && (request.CampaignIds.Count == 0
                            || (request.CampaignIds.Count > 0 && request.CampaignIds.Contains(x.CampaignMember.CampaignId)))
                        && (request.Status.Count == 0
                            || (status.Count > 0 && status.Contains(x.Status))),
                  include: x => x.Include(ma => ma.CampaignMember).ThenInclude(cm => cm.Campaign).Include(ma => ma.Evidences),
                  pageIndex: request.PageIndex,
                  pageSize: request.PageSize,
                  orderBy: request.OrderField,
                  orderByDecensing: request.OrderBy == OrderBy.DESC);

                var pageView = page.ToResponsePagedList<MemberActivityViewModel>(Mapper);
                return new Response<IPagedList<MemberActivityViewModel>>(pageView);
            }
        }
    }
}
