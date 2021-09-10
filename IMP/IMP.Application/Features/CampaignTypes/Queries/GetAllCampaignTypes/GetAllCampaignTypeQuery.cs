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
    public class GetAllCampaignTypeQuery : IRequest<Response<IEnumerable<CampaignTypeViewModel>>>
    {
        public class GetAllCampaignTypeQueryHandler : IRequestHandler<GetAllCampaignTypeQuery, Response<IEnumerable<CampaignTypeViewModel>>>
        {
            private readonly ICampaignTypeRepositoryAsync _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            public GetAllCampaignTypeQueryHandler(ICampaignTypeRepositoryAsync campaignTypeRepositoryAsync, IMapper mapper)
            {
                _campaignTypeRepositoryAsync = campaignTypeRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<IEnumerable<CampaignTypeViewModel>>> Handle(GetAllCampaignTypeQuery request, CancellationToken cancellationToken)
            {
                var campaignTypes = await _campaignTypeRepositoryAsync.GetAllAsync();
                var campaignTypeViews = _mapper.Map<IEnumerable<CampaignTypeViewModel>>(campaignTypes);
                return new Response<IEnumerable<CampaignTypeViewModel>>(campaignTypeViews);
            }
        }
    }
}
