using AutoMapper;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }
    }

    public class CreatePlatformCommandHandler : IRequestHandler<CreatePlatformCommand, Response<PlatformViewModel>>
    {
        private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IFirebaseService _firebaseService;
        public CreatePlatformCommandHandler(IPlatformRepositoryAsync platformRepositoryAsync, IMapper mapper, IFirebaseService firebaseService)
        {
            _platformRepositoryAsync = platformRepositoryAsync;
            _mapper = mapper;
            _firebaseService = firebaseService;
        }

        public async Task<Response<PlatformViewModel>> Handle(CreatePlatformCommand request, CancellationToken token)
        {
            var platform = _mapper.Map<Platform>(request);
            string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), "platforms", request.ImageFile.FileName);
            if (imageFileUrl != null)
            {
                platform.Image = imageFileUrl;
            }
            platform = await _platformRepositoryAsync.AddAsync(platform);

            var platformView = _mapper.Map<PlatformViewModel>(platform);
            return new Response<PlatformViewModel>(platformView);

        }

    }
}
