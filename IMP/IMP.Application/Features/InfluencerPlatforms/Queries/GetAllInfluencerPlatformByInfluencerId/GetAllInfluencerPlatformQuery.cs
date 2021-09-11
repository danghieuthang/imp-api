using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.InfluencerPlatforms.Queries.GetAllInfluencerPlatformByInfluencerId
{
    public class GetAllInfluencerPlatformQuery : IRequest<Response<IEnumerable<InfluencerPlatformViewModel>>>
    {
        public int InfluencerId { get; set; }

        public class GetAllInfluencerPlatformQueryHandler : IRequestHandler<GetAllInfluencerPlatformQuery, Response<IEnumerable<InfluencerPlatformViewModel>>>
        {
            private readonly IGenericRepositoryAsync<int, InfluencerPlatform> _influencerPlatformRepositoryAsync;
            private readonly IMapper _mapper;

            public GetAllInfluencerPlatformQueryHandler(IGenericRepositoryAsync<int, InfluencerPlatform> influencerPlatformRepositoryAsync, IMapper mapper)
            {
                _influencerPlatformRepositoryAsync = influencerPlatformRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<IEnumerable<InfluencerPlatformViewModel>>> Handle(GetAllInfluencerPlatformQuery request, CancellationToken cancellationToken)
            {
                var influencerPlatforms = await _influencerPlatformRepositoryAsync.FindAllAsync(x => x.InfluencerId == request.InfluencerId, x => x.Platform);
                var influencerPlatformViews = _mapper.Map<IEnumerable<InfluencerPlatformViewModel>>(influencerPlatforms);
                return new Response<IEnumerable<InfluencerPlatformViewModel>>(influencerPlatformViews);
            }
        }
    }
}
