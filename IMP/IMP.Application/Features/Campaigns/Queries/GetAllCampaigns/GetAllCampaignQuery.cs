using AutoMapper;
using FluentValidation;
using IMP.Application.Models.Compaign;
using IMP.Application.Extensions;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Domain.Entities;
using IMP.Application.Interfaces;
using IMP.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMP.Application.Models;
using IMP.Application.Validations;

namespace IMP.Application.Features.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignQuery : PageRequest, IListQuery<CampaignViewModel>
    {
        public GetAllCampaignQuery()
        {
            Status = new List<int>();
            TypeIds = new List<int>();
            PlatformIds = new List<int>();
        }
        [FromQuery(Name = "status")]
        public List<int> Status { get; set; }
        [FromQuery(Name = "brand_id")]
        public int? BrandId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }
        [FromQuery(Name = "type_ids")]
        public List<int> TypeIds { get; set; }
        [FromQuery(Name = "platform_ids")]
        public List<int> PlatformIds { get; set; }
        [FromQuery(Name = "name")]
        public string Name { get; set; }
        [FromQuery(Name = "job")]
        public string Job { get; set; }
        public class GetAllCampaignQueryValidator : PageRequestValidator<GetAllCampaignQuery, CampaignViewModel>
        {
        }

        public class GetAllCampaignQueryHandler : ListQueryHandler<GetAllCampaignQuery, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;

            public GetAllCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
            }
            public override async Task<Response<IPagedList<CampaignViewModel>>> Handle(GetAllCampaignQuery request, CancellationToken cancellationToken)
            {
                request.Name = string.IsNullOrEmpty(request.Name) ? "" : request.Name;

                var page = await _campaignRepository.GetPagedList(
                    predicate: x => (request.Status.Count == 0 || (request.Status.Count > 0 && request.Status.Contains(x.Status)))
                        && (request.BrandId == null || (request.BrandId != null && x.BrandId == request.BrandId))
                        && (request.FromDate == null || (request.FromDate != null && x.Created.Date >= request.FromDate))
                        && (request.ToDate == null || (request.ToDate != null && x.Created.Date <= request.ToDate))
                        && (request.TypeIds.Count == 0 || (request.TypeIds.Count > 0 && request.TypeIds.Contains(x.CampaignTypeId.Value)))
                        && (request.PlatformIds.Count == 0 || (request.PlatformIds.Count > 0 && request.PlatformIds.Contains(x.InfluencerConfiguration.PlatformId.Value)))
                        && (string.IsNullOrWhiteSpace(request.Name) || x.Title.ToLower().Contains(request.Name.ToLower())
                            || x.Description.ToLower().Contains(request.Name.ToLower())
                            || x.AdditionalInformation.ToLower().Contains(request.Name.ToLower()))
                        && x.LastModified != null,
                    include: x => x.Include(y => y.Brand)
                        .Include(campaing => campaing.TargetConfiguration)
                        .Include(y => y.CampaignImages)
                        .Include(campaign => campaign.InfluencerConfiguration).ThenInclude(y => y.Platform)
                        .Include(campaign => campaign.InfluencerConfiguration).ThenInclude(y => y.Locations).ThenInclude(z => z.Location)
                        .Include(campaign => campaign.CampaignType),
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
