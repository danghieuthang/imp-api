using AutoMapper;
using IMP.Application.Models.Compaign;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Queries.GetCampaignById
{
    public class GetCampaignByIdQuery : IRequest<Response<CampaignViewModel>>
    {
        public int Id { get; set; }
        public class GetCampaignByIdQueryHandler : IRequestHandler<GetCampaignByIdQuery, Response<CampaignViewModel>>
        {
            private readonly ICampaignRepositoryAsync _campaignRepositoryAsync;
            private readonly IMapper _mapper;

            public GetCampaignByIdQueryHandler(ICampaignRepositoryAsync campaignRepositoryAsync, IMapper mapper)
            {
                _campaignRepositoryAsync = campaignRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<CampaignViewModel>> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
            {
                var campaign = await _campaignRepositoryAsync.GetByIdAsync(request.Id);
                if (campaign == null)
                {
                    throw new KeyNotFoundException($"'{request.Id}' không tồn tại");
                }
                var view = _mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(view);
            }
        }
    }


}
