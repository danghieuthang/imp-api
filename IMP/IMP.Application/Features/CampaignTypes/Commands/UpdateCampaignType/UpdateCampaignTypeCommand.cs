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
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepositoryAsync<CampaignType> _campaignTypeRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly IFirebaseService _firebaseService;

            public UpdateCampaignTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService)
            {
                _unitOfWork = unitOfWork;
                _campaignTypeRepositoryAsync = _unitOfWork.Repository<CampaignType>();
                _mapper = mapper;
                _firebaseService = firebaseService;
            }

            public async Task<Response<CampaignTypeViewModel>> Handle(UpdateCampaignTypeCommand request, CancellationToken cancellationToken)
            {
                var campaignType = await _campaignTypeRepositoryAsync.GetByIdAsync(request.Id);
                if (campaignType != null)
                {
                    campaignType.ParentId = request.ParentId;
                    campaignType.Name = request.Name;
                    campaignType.Description = request.Description;

                    string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "campaign-types");
                    if (imageFileUrl != null)
                    {
                        campaignType.Image = imageFileUrl;
                    }
                    _campaignTypeRepositoryAsync.Update(campaignType);
                    await _unitOfWork.CommitAsync();
                }

                var campaignTypeView = _mapper.Map<CampaignTypeViewModel>(campaignType);
                return new Response<CampaignTypeViewModel>(campaignTypeView);
            }
        }
    }
}
