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

namespace IMP.Application.Features.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignQuery : IListQuery<CampaignViewModel>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderField { get; set; }
        public OrderBy OrderBy { get; set; }

        public class Validator : AbstractValidator<GetAllCampaignQuery>
        {
            public Validator()
            {
                RuleFor(x => x.OrderField).MustValidOrderField(typeof(CampaignViewModel));
            }
        }

        public class GetAllCampaignQueryHandler : ListQueryHandler<GetAllCampaignQuery,CampaignViewModel>
        {
            private readonly IGenericRepositoryAsync<Campaign> _campaignRepositoryAsync;

            public GetAllCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _campaignRepositoryAsync = unitOfWork.Repository<Campaign>();
            }

            public async Task<PagedList<IEnumerable<CampaignViewModel>>> Handle(GetAllCampaignQuery request, CancellationToken cancellationToken)
            {
                var campaigns = await _campaignRepositoryAsync.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.Includes, request.OrderField, request.OrderBy);
                var campaignViews = _mapper.Map<IEnumerable<CampaignViewModel>>(campaigns.Item1);
                return new PagedResponse<IEnumerable<CampaignViewModel>>(campaignViews, request.PageNumber, request.PageSize, totalCount: campaigns.Item2);
            }

            public override async Task<Response<PagedList<CampaignViewModel>>> Handle(GetAllCampaignQuery request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }


}
