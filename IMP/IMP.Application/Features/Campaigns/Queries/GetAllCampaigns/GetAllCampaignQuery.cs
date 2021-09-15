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

namespace IMP.Application.Features.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignQuery : GetAllCampaignParameter, IRequest<PagedResponse<IEnumerable<CampaignViewModel>>>
    {
        public class Validator : AbstractValidator<GetAllCampaignQuery>
        {
            public Validator()
            {
                RuleFor(x => x.OrderField).MustValidOrderField(typeof(CampaignViewModel));
            }
        }

        public class GetAllCampaignQueryHandler : IRequestHandler<GetAllCampaignQuery, PagedResponse<IEnumerable<CampaignViewModel>>>
        {
            private readonly IGenericRepositoryAsync<Campaign> _campaignRepositoryAsync;
            private readonly IMapper _mapper;

            public GetAllCampaignQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _campaignRepositoryAsync = unitOfWork.Repository<Campaign>();
                _mapper = mapper;
            }

            public async Task<PagedResponse<IEnumerable<CampaignViewModel>>> Handle(GetAllCampaignQuery request, CancellationToken cancellationToken)
            {
                var campaigns = await _campaignRepositoryAsync.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.Includes, request.OrderField, request.OrderBy);
                var campaignViews = _mapper.Map<IEnumerable<CampaignViewModel>>(campaigns.Item1);
                return new PagedResponse<IEnumerable<CampaignViewModel>>(campaignViews, request.PageNumber, request.PageSize, totalCount: campaigns.Item2);
            }
        }
    }


}
