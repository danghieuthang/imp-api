using AutoMapper;
using FluentValidation;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Validations;
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
    public class GetCampaignByInfluencerIdForBrandQuery : PageRequest, IListQuery<CampaignViewModel>
    {
        public GetCampaignByInfluencerIdForBrandQuery()
        {
            TypeIds = new List<int>();
        }
        [FromQuery(Name = "status")]
        public CampaignMemberStatus? Status { get; set; }
        [FromQuery(Name = "brand_id")]
        public int? BrandId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }
        [FromQuery(Name = "type_ids")]
        public List<int> TypeIds { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromRoute()]
        public int Id { get; set; }

        public class GetCampaignByInfluencerIdForBrandQueryValidator : PageRequestValidator<GetCampaignByInfluencerIdForBrandQuery, CampaignViewModel>
        {
            public GetCampaignByInfluencerIdForBrandQueryValidator()
            {
                RuleFor(x => x.Status).IsInEnum();
            }
        }

        public class GetCampaignByInfluencerIdForBrandQueryHandler : ListQueryHandler<GetCampaignByInfluencerIdForBrandQuery, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetCampaignByInfluencerIdForBrandQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<IPagedList<CampaignViewModel>>> Handle(GetCampaignByInfluencerIdForBrandQuery request, CancellationToken cancellationToken)
            {
                var page = await _campaignRepository.GetPagedList(
                    predicate: x =>
                        (x.CampaignMembers.Any(cm => cm.InfluencerId == request.Id))
                        && (request.Status == null
                            || (request.Status.HasValue && x.CampaignMembers.Any(cm => cm.InfluencerId == _authenticatedUserService.ApplicationUserId && cm.Status == (int)request.Status.Value)))
                        && (request.BrandId == null || (request.BrandId != null && x.BrandId == request.BrandId))
                        && (request.FromDate == null || (request.FromDate != null && x.Created.Date >= request.FromDate))
                        && (request.ToDate == null || (request.ToDate != null && x.Created.Date <= request.ToDate))
                        && (request.TypeIds.Count == 0 || (request.TypeIds.Count > 0 && request.TypeIds.Contains(x.CampaignTypeId.Value)))
                        && (string.IsNullOrWhiteSpace(request.Name) || x.Title.ToLower().Contains(request.Name.ToLower())
                            || x.Description.ToLower().Contains(request.Name.ToLower())
                            || x.AdditionalInformation.ToLower().Contains(request.Name.ToLower()))
                        && x.LastModified != null,
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
