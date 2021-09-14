using AutoMapper;
using FluentValidation.Results;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
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
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepositoryAsync<Platform> _platformRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;

            public UpdatePlatformCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService)
            {
                _unitOfWork = unitOfWork;
                _platformRepositoryAsync = _unitOfWork.Repository<Platform>();
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<PlatformViewModel>> Handle(UpdatePlatformCommand request, CancellationToken cancellationToken)
            {
                var platform = await _platformRepositoryAsync.GetByIdAsync(request.Id);
                if (platform != null)
                {
                    platform.Name = request.Name;
                    string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "platforms");
                    if (imageFileUrl != null)
                    {
                        platform.Image = imageFileUrl;
                    }
                    _platformRepositoryAsync.Update(platform);
                    await _unitOfWork.CommitAsync();
                }

                var platformView = _mapper.Map<PlatformViewModel>(platform);
                return new Response<PlatformViewModel>(platformView);
            }
        }
    }


}
