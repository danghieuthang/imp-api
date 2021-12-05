using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
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

namespace IMP.Application.Features.Campaigns.Queries.GetCampaignByInfluencerId
{
    public class GetCampaignByBioLinkQuery : PageRequest, IListQuery<CampaignViewModel>
    {
        [FromQuery(Name = "bio_link")]
        public string BioLink { get; set; }
        public class GetCampaignByBioLinkQueryHandler : ListQueryHandler<GetCampaignByBioLinkQuery, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;

            public GetCampaignByBioLinkQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();

            }

            public override async Task<Response<IPagedList<CampaignViewModel>>> Handle(GetCampaignByBioLinkQuery request, CancellationToken cancellationToken)
            {
                var influencer = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Pages.Any(y => y.BioLink.ToLower() == request.BioLink.ToLower()));
                if (influencer == null)
                {
                    return new Response<IPagedList<CampaignViewModel>>(data: null);
                }

                var page = await _campaignRepository.GetPagedList(
                   predicate: x =>
                       (x.CampaignMembers.Any(cm => cm.InfluencerId == influencer.Id && cm.Status == (int)CampaignMemberStatus.Approved))
                       && x.Status >= (int)CampaignStatus.Advertising && x.Status <= (int)CampaignStatus.Closed,
                    include: x =>
                        x.Include(y => y.Brand)
                        .Include(campaing => campaing.InfluencerConfiguration)
                            .ThenInclude(t => t.Locations)
                                .ThenInclude(l => l.Location)
                        .Include(c => c.InfluencerConfiguration)
                            .ThenInclude(i => i.Platform)
                        .Include(y => y.CampaignImages)
                        .Include(campaign => campaign.TargetConfiguration),
                   pageIndex: request.PageIndex,
                   pageSize: request.PageSize,
                   orderBy: request.OrderField,
                   orderByDecensing: request.OrderBy == OrderBy.DESC);
                var pageViews = page.ToResponsePagedList<CampaignViewModel>(Mapper);
                return new Response<IPagedList<CampaignViewModel>>(pageViews);
            }
        }
    }
}
