using AutoMapper;
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
using IMP.Application.Enums;
using IMP.Application.Features.Campaigns;
using IMP.Application.Extensions;

namespace IMP.Application.Features.ApplicationUsers.Queries.GetAllInfluencer
{
    public class GetAllInfluencerSuitableByCampaignQuery : PageRequest, IListQuery<InfluencerViewModel>
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "campaign_id")]
        public int CampaignId { get; set; }

        public class GetAllInfluencerSuitableByCampaignQueryHandler : ListQueryHandler<GetAllInfluencerSuitableByCampaignQuery, InfluencerViewModel>
        {
            public GetAllInfluencerSuitableByCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IPagedList<InfluencerViewModel>>> Handle(GetAllInfluencerSuitableByCampaignQuery request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.InfluencerConfiguration, x => x.InfluencerConfiguration.Locations);
                if (campaign == null)
                {
                    return new Response<IPagedList<InfluencerViewModel>>();
                }

                string name = request.Name != null ? request.Name.ToLower().Trim() : "";

                var influencers = await UnitOfWork.Repository<ApplicationUser>().GetAllWithOrderByStringField(
                        predicate: x => x.BrandId == null
                                && (x.FirstName.ToLower().Contains(name)
                                    || x.LastName.ToLower().Contains(name)
                                    || x.Nickname.ToLower().Contains(name)),

                        orderBy: request.OrderField,
                        orderByDecensing: request.OrderBy == OrderBy.DESC,
                        include: x => x.Include(y => y.Ranking).Include(y => y.InfluencerPlatforms).ThenInclude(y => y.Platform)).ToListAsync();

                var page = influencers.Where(x =>
                {
                    var errors = CampaignUtils.CheckSuitability(campaign, x);
                    return errors.Count == 0;
                }).ToPagedList(pageIndex: request.PageIndex, pageSize: request.PageSize);

                var pageView = page.ToResponsePagedList<InfluencerViewModel>(Mapper);
                return new Response<IPagedList<InfluencerViewModel>>(pageView);

            }
        }
    }
}
