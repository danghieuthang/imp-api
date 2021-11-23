using AutoMapper;
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
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace IMP.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CampaignMilestoneRequest
    {
        public int MilestoneId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

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
        public int ApplicationUserId { get; set; }
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
        public decimal PrizeMoney { get; set; }
        public bool IsPrizePerVoucher { get; set; }
        #region timeline
        public DateTime? Openning { get; set; }
        public DateTime? Applying { get; set; }
        public DateTime? Advertising { get; set; }
        public DateTime? Evaluating { get; set; }
        public DateTime? Announcing { get; set; }
        public DateTime? Closed { get; set; }
        #endregion

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
            var applicationUser = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == request.ApplicationUserId);

            var campaign = _mapper.Map<Campaign>(request);
            campaign.CreatedById = request.ApplicationUserId;
            campaign.BrandId = applicationUser.BrandId.Value;

            campaign.Status = (int)CampaignStatus.Pending;
            campaign = await _campaignRepository.AddAsync(campaign);
            await UnitOfWork.CommitAsync();

            await CreateCampaignImages(request.Images, campaign.Id);
            await UnitOfWork.CommitAsync();

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
        }

        //private async Task CreateCampaignMilestones(List<CampaignMilestoneRequest> campaignMilestoneRequests, int campaignId)
        //{
        //    var campaignMilestones = campaignMilestoneRequests.Select(x => new CampaignMilestone
        //    {
        //        CampaignId = campaignId,
        //        FromDate = x.FromDate,
        //        ToDate = x.ToDate,
        //        MilestoneId = x.MilestoneId
        //    }).ToList();
        //    await UnitOfWork.Repository<CampaignMilestone>().AddManyAsync(campaignMilestones);
        //}

    }
}
