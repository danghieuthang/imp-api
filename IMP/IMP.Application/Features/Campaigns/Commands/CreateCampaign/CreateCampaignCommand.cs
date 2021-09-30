﻿using AutoMapper;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Interfaces;
using System.Collections.Generic;
using IMP.Application.Enums;

namespace IMP.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class Image
    {
        public int Position { get; set; }
        public string Url { get; set; }
    }
    public class CreateCampaignCommand : ICommand<CampaignViewModel>
    {
        public int PlatformId { get; set; }
        public int CampaignTypeId { get; set; }
        [JsonIgnore]
        public int BrandId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AdditionalInfomation { get; set; }
        public string ProductInformation { get; set; }
        public string Reward { get; set; }
        public string ReferralWebsite { get; set; }
        public string Keywords { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxInfluencer { get; set; }
        public string Condition { get; set; }
        [JsonProperty("campaign_images")]
        public List<Image> Images { get; set; }
    }

    public class CreateCampaignCommandHandler : CommandHandler<CreateCampaignCommand, CampaignViewModel>
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IMapper _mapper;
        public CreateCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _campaignRepository = unitOfWork.Repository<Campaign>();
            _mapper = mapper;
        }

        public override async Task<Response<CampaignViewModel>> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            var campaign = _mapper.Map<Campaign>(request);
            campaign.Status = (int)CampaignStatus.OPEN;
            campaign = await _campaignRepository.AddAsync(campaign);
            await UnitOfWork.CommitAsync();

            await CreateCampaignImages(request.Images, campaign.Id);

            campaign = await _campaignRepository.FindSingleAsync(x => x.Id == campaign.Id, x => x.CampaignImages);
            var campaignView = _mapper.Map<CampaignViewModel>(campaign);
            return new Response<CampaignViewModel>(campaignView);
        }
        private async Task CreateCampaignImages(List<Image> images, int campaignId)
        {
            var campaignImages = new List<CampaignImage>();
            foreach (var image in images)
            {
                campaignImages.Add(new CampaignImage
                {
                    Position = image.Position,
                    Url = image.Url,
                    CampaignId = campaignId
                });
            }
            await UnitOfWork.Repository<CampaignImage>().AddManyAsync(campaignImages);
            await UnitOfWork.CommitAsync();
        }

    }
}
