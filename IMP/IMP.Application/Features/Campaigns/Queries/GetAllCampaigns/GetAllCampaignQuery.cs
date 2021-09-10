using AutoMapper;
using IMP.Application.DTOs.Compaign;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Queries.GetAllCampaigns
{
    public class GetAllCampaignQuery : GetAllCampaignParameter, IRequest<PagedResponse<IEnumerable<CampaignViewModel>>>
    {
    }

    public class GetAllCampaignQueryHandler : IRequestHandler<GetAllCampaignQuery, PagedResponse<IEnumerable<CampaignViewModel>>>
    {
        private readonly ICampaignRepositoryAsync _campaignRepositoryAsync;
        private readonly IMapper _mapper;

        public GetAllCampaignQueryHandler(ICampaignRepositoryAsync campaignRepositoryAsync, IMapper mapper)
        {
            _campaignRepositoryAsync = campaignRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<CampaignViewModel>>> Handle(GetAllCampaignQuery request, CancellationToken cancellationToken)
        {
            var campaigns = await _campaignRepositoryAsync.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.Includes, request.OrderField, request.OrderBy);
            var campaignViews = _mapper.Map<IEnumerable<CampaignViewModel>>(campaigns);
            return new PagedResponse<IEnumerable<CampaignViewModel>>(campaignViews, request.PageNumber, request.PageSize);
        }
    }
}
