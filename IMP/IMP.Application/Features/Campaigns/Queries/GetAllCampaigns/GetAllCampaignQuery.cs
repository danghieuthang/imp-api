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
                    include: x => x.Include(campaign => campaign.CampaignImages)
                                    .Include(campaign => campaign.CampaignMilestones), 
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
