using AutoMapper;
using IMP.Application.Models.ViewModels;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepositoryAsync<Platform> _platformRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IFirebaseService _firebaseService;
        public CreatePlatformCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService)
        {
            _unitOfWork = unitOfWork;
            _platformRepositoryAsync = _unitOfWork.Repository<Platform>();
            _mapper = mapper;
            _firebaseService = firebaseService;
        }

        public async Task<Response<PlatformViewModel>> Handle(CreatePlatformCommand request, CancellationToken token)
        {
            var platform = _mapper.Map<Platform>(request);
            string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "platforms");
            if (imageFileUrl != null)
            {
                platform.Image = imageFileUrl;
            }
            platform = await _platformRepositoryAsync.AddAsync(platform);
            await _unitOfWork.CommitAsync();

            var platformView = _mapper.Map<PlatformViewModel>(platform);
            return new Response<PlatformViewModel>(platformView);

        }

    }
}
