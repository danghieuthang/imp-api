using AutoMapper;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Platforms.Queries
{
    public class GetPlatformByIdQuery : IRequest<Response<PlatformViewModel>>
    {
        public int Id { get; set; }
        public class GetPlatformByIDQueryHandler : IRequestHandler<GetPlatformByIdQuery, Response<PlatformViewModel>>
        {
            private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
            private readonly IMapper _mapper;
            public GetPlatformByIDQueryHandler(IPlatformRepositoryAsync platformRepositoryAsync, IMapper mapper)
            {
                _platformRepositoryAsync = platformRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<PlatformViewModel>> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
            {
                var platform = await _platformRepositoryAsync.GetByIdAsync(request.Id);
                if (platform == null)
                {
                    throw new KeyNotFoundException($"'{request.Id}' không tồn tại.");
                }
                var platformView = _mapper.Map<PlatformViewModel>(platform);
                return new Response<PlatformViewModel>(platformView);
            }
        }
    }
}
