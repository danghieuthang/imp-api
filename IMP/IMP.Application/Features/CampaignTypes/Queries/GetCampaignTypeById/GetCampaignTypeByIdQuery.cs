using AutoMapper;
using IMP.Application.Models.ViewModels;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignTypes.Queries.GetCampaignTypeById
{
    public class GetCampaignTypeByIdQuery : IRequest<Response<CampaignTypeViewModel>>
    {
        public int Id { get; set; }
        public class GetCampaignTypeByIdQueryHandler : IRequestHandler<GetCampaignTypeByIdQuery, Response<CampaignTypeViewModel>>
        {
            private readonly ICampaignTypeRepositoryAsync _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            public GetCampaignTypeByIdQueryHandler(ICampaignTypeRepositoryAsync campaignTypeRepositoryAsync, IMapper mapper)
            {
                _campaignTypeRepositoryAsync = campaignTypeRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<CampaignTypeViewModel>> Handle(GetCampaignTypeByIdQuery request, CancellationToken cancellationToken)
            {
                var campaignType = await _campaignTypeRepositoryAsync.GetByIdAsync(request.Id);
                if (campaignType == null)
                {
                    throw new KeyNotFoundException($"'{request.Id}' không tồn tại");
                }

                var campaignTypeView = _mapper.Map<CampaignTypeViewModel>(campaignType);
                return new Response<CampaignTypeViewModel>(campaignTypeView);
            }
        }
    }
}
