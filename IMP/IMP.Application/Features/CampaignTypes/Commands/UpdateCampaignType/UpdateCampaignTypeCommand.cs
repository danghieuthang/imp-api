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

namespace IMP.Application.Features.CampaignTypes.Commands.UpdateCampaignType
{
    public class UpdateCampaignTypeCommand : IRequest<Response<CampaignTypeViewModel>>
    {
        [FromForm(Name = "id")]
        public int Id { get; set; }
        [FromForm(Name = "parent_id")]
        public int? ParentId { get; set; }

        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }

        [FromForm(Name = "description")]
        public string Description { get; set; }

        public class UpdateCampaignTypeCommandHandler : IRequestHandler<UpdateCampaignTypeCommand, Response<CampaignTypeViewModel>>
        {
            private readonly ICampaignTypeRepositoryAsync _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;

            public UpdateCampaignTypeCommandHandler(ICampaignTypeRepositoryAsync campaignTypeRepositoryAsync, IMapper mapper, IFirebaseService firebaseService)
            {
                _campaignTypeRepositoryAsync = campaignTypeRepositoryAsync;
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<CampaignTypeViewModel>> Handle(UpdateCampaignTypeCommand request, CancellationToken cancellationToken)
            {
                var campaignType = _mapper.Map<CampaignType>(request);
                string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "campaign-types");
                if (imageFileUrl != null)
                {
                    campaignType.Image = imageFileUrl;
                }
                await _campaignTypeRepositoryAsync.UpdateAsync(campaignType);
                var campaignTypeView = _mapper.Map<CampaignTypeViewModel>(campaignType);
                return new Response<CampaignTypeViewModel>(campaignTypeView);
            }
        }
    }
}
