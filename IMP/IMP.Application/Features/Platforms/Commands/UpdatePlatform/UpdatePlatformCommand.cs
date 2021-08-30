using AutoMapper;
using FluentValidation.Results;
using IMP.Application.DTOs;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Exceptions;
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

namespace IMP.Application.Features.Platforms.Commands.UpdatePlatform
{
    public class UpdatePlatformCommand : IRequest<Response<PlatformViewModel>>
    {
        public int Id { get; set; }
        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }
        public class UpdatePlatformCommandHandler : IRequestHandler<UpdatePlatformCommand, Response<PlatformViewModel>>
        {
            private readonly IPlatformRepositoryAsync _platformRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;

            public UpdatePlatformCommandHandler(IPlatformRepositoryAsync platformRepositoryAsync, IMapper mapper, IFirebaseService firebaseService)
            {
                _platformRepositoryAsync = platformRepositoryAsync;
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<PlatformViewModel>> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
            {
                var platform = _mapper.Map<Platform>(request);
                string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "platforms");
                if (imageFileUrl != null)
                {
                    platform.Image = imageFileUrl;
                }
                await _platformRepositoryAsync.UpdateAsync(platform);

                var platformView = _mapper.Map<PlatformViewModel>(platform);
                return new Response<PlatformViewModel>(platformView);
            }
        }
    }


}
