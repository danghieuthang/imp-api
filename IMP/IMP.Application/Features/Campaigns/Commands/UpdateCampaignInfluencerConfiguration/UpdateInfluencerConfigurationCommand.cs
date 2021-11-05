using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaignInfluencerConfiguration
{
    public class UpdateInfluencerConfigurationCommand : ICommand<CampaignViewModel>
    {
        public int CampaignId { get; set; }
        public int PlatformId { get; set; }
        public int NumberOfInfluencer { get; set; }
        public int LevelId { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool UnlimitedAge { get; set; }
        public Genders Gender { get; set; }
        public int? GroupId { get; set; }

        public List<string> Interests { get; set; }
        public List<string> Jobs { get; set; }
        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }
        public List<string> Others { get; set; }
        public List<LocationRequest> Locations { get; set; }
    }

    public class UpdateInfluencerConfigurationCommandHandler : CommandHandler<UpdateInfluencerConfigurationCommand, CampaignViewModel>
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IGenericRepository<InfluencerConfigurationLocation> _influencerConfigurationLocation;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public UpdateInfluencerConfigurationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
        {
            _authenticatedUserService = authenticatedUserService;
            _campaignRepository = unitOfWork.Repository<Campaign>();
            _influencerConfigurationLocation = unitOfWork.Repository<InfluencerConfigurationLocation>();
        }

        public override async Task<Response<CampaignViewModel>> Handle(UpdateInfluencerConfigurationCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(_authenticatedUserService.ApplicationUserId);

            var campaign = await _campaignRepository.FindSingleAsync(x => x.Id == request.CampaignId, x => x.InfluencerConfiguration, x => x.InfluencerConfiguration.Locations);
            if (campaign != null && campaign.BrandId == applicationUser.BrandId)
            {
                InfluencerConfiguration influencerConfiguration = new();
                // if campaign already has influencer configuration
                if (campaign.InfluencerConfiguration != null)
                {
                    Mapper.Map(request, influencerConfiguration);
                    influencerConfiguration.Id = campaign.InfluencerConfiguration.Id;

                    // process locations
                    var domainLocationsDict = campaign.InfluencerConfiguration.Locations.ToDictionary(x => x.LocationId);
                    var requestLocations = influencerConfiguration.Locations;

                    // get locations
                    requestLocations = requestLocations.Select(x =>
                    {
                        if (domainLocationsDict.ContainsKey(x.LocationId))
                        {
                            return domainLocationsDict.GetValueOrDefault(x.LocationId);
                        }
                        return x;
                    }).ToList();

                    // remove location in domain but not in request
                    var requestLocationIds = requestLocations.Select(x => x.LocationId);
                    foreach (var location in domainLocationsDict.Values)
                    {
                        if (!requestLocationIds.Contains(location.LocationId))
                        {
                            _influencerConfigurationLocation.DeleteCompletely(location);
                        }
                    }

                    influencerConfiguration.Locations = requestLocations;
                }
                else
                {
                    influencerConfiguration = Mapper.Map<InfluencerConfiguration>(request);
                }

                campaign.InfluencerConfiguration = influencerConfiguration;

                _campaignRepository.Update(campaign);
                await UnitOfWork.CommitAsync();

                var campaignView = Mapper.Map<CampaignViewModel>(campaign);
                return new Response<CampaignViewModel>(campaignView);
            }
            throw new ValidationException(new ValidationError("", "Không có quyền cập nhật."));
        }
    }

}
