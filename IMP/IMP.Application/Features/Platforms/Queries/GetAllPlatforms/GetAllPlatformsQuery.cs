using AutoMapper;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Platforms.Queries.GetAllPlatforms
{
    public class GetAllPlatformsQuery : IRequest<Response<IEnumerable<PlatformViewModel>>>
    {

    }

    public class GetAllPlatformsQueryHanlder : IRequestHandler<GetAllPlatformsQuery, Response<IEnumerable<PlatformViewModel>>>
    {
        private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllPlatformsQueryHanlder(IPlatformRepositoryAsync platformRepositoryAsync, IMapper mapper)
        {
            _platformRepositoryAsync = platformRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<PlatformViewModel>>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
        {
            var platforms = await _platformRepositoryAsync.GetAllAsync();
            var platformViewModel = _mapper.Map<IEnumerable<PlatformViewModel>>(platforms);
            return new Response<IEnumerable<PlatformViewModel>>(platformViewModel);
        }
    }
}
