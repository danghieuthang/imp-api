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
    public class CreateCampaignTypeCommand : ICommand<CampaignTypeViewModel>
    {
        [FromForm(Name = "parent_id")]
        public int? ParentId { get; set; }

        [FromForm(Name = "name")]
        public string Name { get; set; }

        [FromForm(Name = "image")]
        public IFormFile ImageFile { get; set; }

        [FromForm(Name = "description")]
        public string Description { get; set; }

        public class CreateCampaignTypeCommandHandler : CommandHandler<CreateCampaignTypeCommand, CampaignTypeViewModel>
        {
            private readonly IGenericRepository<CampaignType> _campaignTypeRepositoryAsync;
            private readonly IFirebaseService _firebaseService;
            public CreateCampaignTypeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseService firebaseService) : base(unitOfWork, mapper)
            {
                _campaignTypeRepositoryAsync = unitOfWork.Repository<CampaignType>();
                _firebaseService = firebaseService;
            }

            public override async Task<Response<CampaignTypeViewModel>> Handle(CreateCampaignTypeCommand request, CancellationToken cancellationToken)
            {
                var campaignType = Mapper.Map<CampaignType>(request);
                if (request.ImageFile != null)
                {
                    string imageFileUrl = await _firebaseService.UploadFile(request.ImageFile.OpenReadStream(), request.ImageFile.FileName, "admin", "campaign-types");
                    if (imageFileUrl != null)
                    {
                        campaignType.Image = imageFileUrl;
                    }
                }

                campaignType = await _campaignTypeRepositoryAsync.AddAsync(campaignType);
                await UnitOfWork.CommitAsync();
                var campaignTypeView = Mapper.Map<CampaignTypeViewModel>(campaignType);
                return new Response<CampaignTypeViewModel>(campaignTypeView);
            }

        }
    }
}
