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

namespace IMP.Application.Features.Platforms.Commands.CreatePlatform
{
    public class CreatePlatformCommand : IRequest<Response<PlatformViewModel>>
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class CreatePlatformCommandHandler : IRequestHandler<CreatePlatformCommand, Response<PlatformViewModel>>
    {
        private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
        private readonly IMapper _mapper;
        public CreatePlatformCommandHandler(IPlatformRepositoryAsync platformRepositoryAsync, IMapper mapper)
        {
            _platformRepositoryAsync = platformRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<PlatformViewModel>> Handle(CreatePlatformCommand request, CancellationToken token)
        {
            var platform = _mapper.Map<Platform>(request);
            platform = await _platformRepositoryAsync.AddAsync(platform);

            var platformView = _mapper.Map<PlatformViewModel>(platform);
            return new Response<PlatformViewModel>(platformView);

        }

    }
}
