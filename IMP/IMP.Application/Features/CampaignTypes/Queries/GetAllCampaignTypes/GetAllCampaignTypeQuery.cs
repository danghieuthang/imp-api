using AutoMapper;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Features.CampaignTypes.Queries.GetAllCampaignTypes;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignTypes.Queries
{
    public class GetAllCampaignTypeQuery : GetAllCampaignTypeParameter, IRequest<PagedResponse<IEnumerable<CampaignTypeViewModel>>>
    {
        public class GetAllCampaignTypeQueryHandler : IRequestHandler<GetAllCampaignTypeQuery, PagedResponse<IEnumerable<CampaignTypeViewModel>>>
        {
            private readonly ICampaignTypeRepositoryAsync _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            public GetAllCampaignTypeQueryHandler(ICampaignTypeRepositoryAsync campaignTypeRepositoryAsync, IMapper mapper)
            {
                _campaignTypeRepositoryAsync = campaignTypeRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<PagedResponse<IEnumerable<CampaignTypeViewModel>>> Handle(GetAllCampaignTypeQuery request, CancellationToken cancellationToken)
            {
                var campaignTypes = await _campaignTypeRepositoryAsync.GetPagedReponseAsync(request.PageNumber, request.PageSize);
                var campaignTypeViews = _mapper.Map<IEnumerable<CampaignTypeViewModel>>(campaignTypes);
                return new PagedResponse<IEnumerable<CampaignTypeViewModel>>(campaignTypeViews, request.PageNumber, request.PageSize);
            }
        }
    }
}
