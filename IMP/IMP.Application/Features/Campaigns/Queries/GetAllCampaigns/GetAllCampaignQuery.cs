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

namespace IMP.Application.Features.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignQuery : IListQuery<CampaignViewModel>
    {
        [FromQuery(Name ="page_index")]
        public int PageIndex { get; set; }
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; }

        [FromQuery(Name = "order_field")]
        public string OrderField { get; set; }
        [FromQuery(Name ="order_by")]
        public OrderBy OrderBy { get; set; }

        public class Validator : AbstractValidator<GetAllCampaignQuery>
        {
            public Validator()
            {
                RuleFor(x => x.OrderField).MustValidOrderField(typeof(CampaignViewModel));
            }
        }

        public class GetAllCampaignQueryHandler : ListQueryHandler<GetAllCampaignQuery, CampaignViewModel>
        {
            private readonly IGenericRepositoryAsync<Campaign> _campaignRepositoryAsync;

            public GetAllCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepositoryAsync = unitOfWork.Repository<Campaign>();
            }
            public override async Task<Response<IPagedList<CampaignViewModel>>> Handle(GetAllCampaignQuery request, CancellationToken cancellationToken)
            {

                var page = await _campaignRepositoryAsync.GetPagedList(pageIndex: request.PageIndex, pageSize: request.PageSize, orderBy: request.OrderField, orderByDecensing: request.OrderBy == OrderBy.DESC);
                var pageViews = page.ToResponsePagedList<CampaignViewModel>(Mapper);
                return new Response<IPagedList<CampaignViewModel>>(pageViews);
            }
        }
    }


}
