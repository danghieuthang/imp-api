using AutoMapper;
using IMP.Application.Models.Compaign;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.CreateCampaign
{
    public class CreateCampaignCommand : IRequest<Response<CampaignViewModel>>
    {
        public int PlatformId { get; set; }
        public int CampaignTypeId { get; set; }
        [JsonIgnore]
        public int BrandId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AditionalInfomation { get; set; }
        public string Reward { get; set; }
        public string ReferalWebsite { get; set; }
        public string Keywords { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxInfluencer { get; set; }
        public int Status { get; set; }
        public string Condition { get; set; }
    }

    public class CreateCampaignCommandHandler : IRequestHandler<CreateCampaignCommand, Response<CampaignViewModel>>
    {
        private readonly ICampaignRepositoryAsync _campaignRepositoryAsync;
        private readonly IMapper _mapper;
        public CreateCampaignCommandHandler(ICampaignRepositoryAsync campaignRepositoryAsync, IMapper mapper)
        {
            _campaignRepositoryAsync = campaignRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<CampaignViewModel>> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            var campaign = _mapper.Map<Campaign>(request);
            campaign = await _campaignRepositoryAsync.AddAsync(campaign);

            var campaignView = _mapper.Map<CampaignViewModel>(campaign);
            return new Response<CampaignViewModel>(campaignView);
        }

    }
}
