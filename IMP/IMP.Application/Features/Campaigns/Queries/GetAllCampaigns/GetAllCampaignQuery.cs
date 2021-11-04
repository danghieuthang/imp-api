﻿using AutoMapper;
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
        [FromQuery(Name = "status")]
        public CampaignStatus? Status { get; set; }
        [FromQuery(Name = "brand_id")]
        public int? BrandId { get; set; }
        [FromQuery(Name = "from_date")]
        public DateTime? FromDate { get; set; }
        [FromQuery(Name = "to_date")]
        public DateTime? ToDate { get; set; }
        [FromQuery(Name = "campaign_type_id")]
        public int? CampaignTypeId { get; set; }
        [FromQuery(Name = "platform_id")]
        public int? PlatformId { get; set; }
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

                var page = await _campaignRepository.GetPagedList(
                    predicate: x => (request.Status == null || (request.Status != null && x.Status == (int)request.Status.Value))
                        && (request.BrandId == null || (request.BrandId != null && x.BrandId == request.BrandId))
                        && (request.FromDate == null || (request.FromDate != null && x.Created.Date >= request.FromDate))
                        && (request.ToDate == null || (request.ToDate != null && x.Created.Date <= request.ToDate))
                        && (request.CampaignTypeId == null || (request.CampaignTypeId != null && x.CampaignTypeId == request.CampaignTypeId))
                        && (request.PlatformId == null || (request.PlatformId != null && x.InfluencerConfiguration.PlatformId == request.PlatformId)),
                    include: x => x.Include(campaign => campaign.CampaignImages),
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
