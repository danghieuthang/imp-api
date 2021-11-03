﻿using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaign
{
    public class UpdateCampaignInformationCommand : ICommand<CampaignViewModel>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AdditionalInformation { get; set; }
        public List<CampaignImageRequest> Images { get; set; }
        public string QA { get; set; }
        public List<string> Websites { get; set; }
        public List<string> Fanpages { get; set; }

        #region Timeline

        public DateTime Openning { get; set; }
        public DateTime Applying { get; set; }
        public DateTime Advertising { get; set; }
        public DateTime Evaluating { get; set; }
        public DateTime Announcing { get; set; }
        public DateTime Closed { get; set; }
        #endregion

        #region Product/service configuration

        public int CampaignTypeId { get; set; }
        public List<ProductRequest> Products { get; set; }
        public string Keywords { get; set; }
        public string Hashtags { get; set; }
        public string ProductInformation { get; set; }
        public string SampleContent { get; set; }

        #endregion

        #region Reward configuration
        public List<CampaignRewardRequest> DefaultRewards { get; set; }
        public List<CampaignRewardRequest> BestInfluencerRewards { get; set; }
        #endregion
    }

    public class UpdateCampaignInformationCommandHandler : CommandHandler<UpdateCampaignInformationCommand, CampaignViewModel>
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IGenericRepository<CampaignReward> _campaignRewardRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<CampaignImage> _campaignImageRepository;
        public UpdateCampaignInformationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _campaignRepository = unitOfWork.Repository<Campaign>();
            _campaignRewardRepository = unitOfWork.Repository<CampaignReward>();
            _productRepository = unitOfWork.Repository<Product>();
            _campaignImageRepository = unitOfWork.Repository<CampaignImage>();
        }

        public override async Task<Response<CampaignViewModel>> Handle(UpdateCampaignInformationCommand request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.FindSingleAsync(predicate: x => x.Id == request.Id,
                include: campaigns => campaigns.Include(campaign => campaign.CampaignImages)
                                        .Include(x => x.Products)
                                        .Include(x => x.CampaignRewards));
            if (campaign != null)
            {
                DeleteBeforeUpdate(campaign);
                Mapper.Map(request, campaign);
                // Process Campaign Rewards
                var defaultRewards = request.DefaultRewards.Select(x =>
                    new CampaignReward
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Currency = "VND",
                        IsDefaultReward = true,
                        CampaignId = campaign.Id
                    }).ToList();

                campaign.CampaignRewards = defaultRewards.Union(request.BestInfluencerRewards.Select(x =>
                     new CampaignReward
                     {
                         Name = x.Name,
                         Price = x.Price,
                         Currency = "VND",
                         IsDefaultReward = false,
                         CampaignId = campaign.Id
                     })).ToList();

                _campaignRepository.Update(campaign);
                await UnitOfWork.CommitAsync();
                var campaignView = Mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(campaignView);
            }
            throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));
        }


        private void DeleteBeforeUpdate(Campaign campaign)
        {
            // delete images
            foreach (var image in campaign.CampaignImages)
            {
                _campaignImageRepository.Delete(image);
            }
            // Delete products
            foreach (var product in campaign.Products)
            {
                _productRepository.Delete(product);
            }
            //Delete reward
            foreach (var reward in campaign.CampaignRewards)
            {
                _campaignRewardRepository.Delete(reward);
            }
        }
    }
}