using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        [JsonProperty("images")]
        public List<CampaignImageRequest> CampaignImages { get; set; }
        public string QA { get; set; }
        public List<string> Websites { get; set; }
        public List<string> Fanpages { get; set; }

        #region Timeline

        public DateTime? OpeningDate { get; set; }
        public DateTime? ApplyingDate { get; set; }
        public DateTime? AdvertisingDate { get; set; }
        public DateTime? EvaluatingDate { get; set; }
        public DateTime? AnnouncingDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        #endregion

        #region Product/service configuration

        public int? CampaignTypeId { get; set; }
        public List<ProductRequest> Products { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Hashtags { get; set; }
        public string ProductInformation { get; set; }
        public string SampleContent { get; set; }

        #endregion

        #region Reward configuration
        public List<CampaignRewardRequest> DefaultRewards { get; set; }
        public List<CampaignRewardRequest> BestInfluencerRewards { get; set; }
        public List<CampaignVoucherRequest> Vouchers { get; set; }
        #endregion
    }

    public class UpdateCampaignInformationCommandHandler : CommandHandler<UpdateCampaignInformationCommand, CampaignViewModel>
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IGenericRepository<CampaignReward> _campaignRewardRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<CampaignImage> _campaignImageRepository;
        private readonly IGenericRepository<CampaignVoucher> _campaignVoucherRepository;
        public UpdateCampaignInformationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _campaignRepository = unitOfWork.Repository<Campaign>();
            _campaignRewardRepository = unitOfWork.Repository<CampaignReward>();
            _productRepository = unitOfWork.Repository<Product>();
            _campaignImageRepository = unitOfWork.Repository<CampaignImage>();
            _campaignVoucherRepository = unitOfWork.Repository<CampaignVoucher>();
        }

        public override async Task<Response<CampaignViewModel>> Handle(UpdateCampaignInformationCommand request, CancellationToken cancellationToken)
        {
            var campaign = await _campaignRepository.FindSingleAsync(predicate: x => x.Id == request.Id,
                include: campaigns => campaigns.Include(campaign => campaign.CampaignImages)
                                        .Include(x => x.Products)
                                        .Include(x => x.CampaignRewards)
                                        .Include(x => x.Vouchers));

          

            if (campaign != null)
            {

                if (campaign.Status != (int)CampaignStatus.Draft && campaign.Status != (int)CampaignStatus.Pending && campaign.Status != (int)CampaignStatus.Approved)
                {
                    throw new ValidationException(new ValidationError("id", "Không thể chỉnh sửa chiến dịch này."));
                }

                await DeleteBeforeUpdate(campaign, request);
                Mapper.Map(request, campaign);

                // Process Campaign Rewards
                if (request.DefaultRewards != null && request.BestInfluencerRewards != null)
                {
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
                }

                if (request.Vouchers != null)
                {
                    campaign.Vouchers = campaign.Vouchers.Select(x => { x.CampaignId = campaign.Id; return x; }).ToList();
                }

                // If approved campaign then change status to pending
                if (campaign.Status == (int)CampaignStatus.Approved)
                {
                    campaign.Status = (int)CampaignStatus.Pending;
                }

                _campaignRepository.Update(campaign);
                await UnitOfWork.CommitAsync();
                var campaignView = Mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(campaignView);
            }
            throw new ValidationException(new ValidationError("id", "Chiến dịch không tồn tại."));
        }


        private async Task DeleteBeforeUpdate(Campaign campaign, UpdateCampaignInformationCommand request)
        {
            // delete images
            if (request.CampaignImages != null)
            {
                foreach (var image in campaign.CampaignImages)
                {
                    _campaignImageRepository.DeleteCompletely(image);
                }
            }

            // Delete products
            if (request.Products != null)
            {
                foreach (var product in campaign.Products)
                {
                    _productRepository.DeleteCompletely(product);
                }
            }

            //Delete reward
            if (request.DefaultRewards != null || request.BestInfluencerRewards != null)
            {
                foreach (var reward in campaign.CampaignRewards)
                {
                    _campaignRewardRepository.DeleteCompletely(reward);
                }
            }

            // Delete vouchers
            if (request.Vouchers != null)
            {
                foreach (var voucher in campaign.Vouchers)
                {
                    _campaignVoucherRepository.DeleteCompletely(voucher);
                }
            }
        }
    }
}
