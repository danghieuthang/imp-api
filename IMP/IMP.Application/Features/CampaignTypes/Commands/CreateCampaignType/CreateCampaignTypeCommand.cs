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
using AutoMapper;
using IMP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IMP.Application.Interfaces;

namespace IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType
{
    public class CreateCampaignTypeCommand : IRequest<Response<CampaignTypeViewModel>>
    {
        [FromForm(Name = "parent_id")]
        public int? ParentId { get; set; }

        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }

        [FromForm(Name = "description")]
        public string Description { get; set; }

        public class CreateCampaignTypeCommandHandler : IRequestHandler<CreateCampaignTypeCommand, Response<CampaignTypeViewModel>>
        {
            private readonly ICampaignTypeRepositoryAsync _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;
            public CreateCampaignTypeCommandHandler(ICampaignTypeRepositoryAsync campaignTypeReponsitoryAync, IMapper mapper, IFirebaseService firebaseService)
            {
                _campaignTypeRepositoryAsync = campaignTypeReponsitoryAync;
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<CampaignTypeViewModel>> Handle(CreateCampaignTypeCommand request, CancellationToken cancellationToken)
            {
                var campaignType = _mapper.Map<CampaignType>(request);
                string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "campaign-types");
                if (imageFileUrl != null)
                {
                    campaignType.Image = imageFileUrl;
                }
                campaignType = await _campaignTypeRepositoryAsync.AddAsync(campaignType);
                var campaignTypeView = _mapper.Map<CampaignTypeViewModel>(campaignType);
                return new Response<CampaignTypeViewModel>(campaignTypeView);
            }
        }
    }
}
