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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.UpdateCampaignTargetConfiguration
{
    public class UpdateTargetConfigurationCommand : ICommand<CampaignViewModel>
    {
        public int CampaignId { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public bool UnlimitedAge { get; set; }
        public Genders Gender { get; set; }
        public List<string> Interests { get; set; }
        public List<string> Jobs { get; set; }

        public bool? ChildStatus { get; set; }
        public bool? MaritalStatus { get; set; }
        public bool? Pregnant { get; set; }
        public List<string> Purposes { get; set; }
        public List<LocationRequest> Locations { get; set; }
    }
    public class CreateCampaignTargetCommandHandler : CommandHandler<UpdateTargetConfigurationCommand, CampaignViewModel>
    {
        private readonly IGenericRepository<Campaign> _campaignRepository;
        private readonly IGenericRepository<TargetConfigurationLocation> _targetConfigurationLocation;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        public CreateCampaignTargetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
        {
            _campaignRepository = unitOfWork.Repository<Campaign>();
            _authenticatedUserService = authenticatedUserService;
            _targetConfigurationLocation = unitOfWork.Repository<TargetConfigurationLocation>();
        }

        public override async Task<Response<CampaignViewModel>> Handle(UpdateTargetConfigurationCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(_authenticatedUserService.ApplicationUserId);

            var campaign = await _campaignRepository.FindSingleAsync(x => x.Id == request.CampaignId, x => x.TargetConfiguration, x => x.TargetConfiguration.Locations);
            if (campaign != null && campaign.BrandId == applicationUser.BrandId)
            {
                if (campaign.Status != (int)CampaignStatus.Draft && campaign.Status != (int)CampaignStatus.Pending && campaign.Status != (int)CampaignStatus.Approved && campaign.Status != (int)CampaignStatus.Cancelled)
                {
                    throw new ValidationException(new ValidationError("id", "Không thể chỉnh sửa chiến dịch này."));
                }

                TargetConfiguration targetConfiguration = new();
                // if campaign already has target configuration
                if (campaign.TargetConfiguration != null)
                {
                    Mapper.Map(request, targetConfiguration);
                    targetConfiguration.Id = campaign.TargetConfiguration.Id;

                    // process locations
                    var domainLocationsDict = campaign.TargetConfiguration.Locations.ToDictionary(x => x.LocationId);
                    var requestLocations = targetConfiguration.Locations;

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
                            _targetConfigurationLocation.DeleteCompletely(location);
                        }
                    }

                    targetConfiguration.Locations = requestLocations;
                }
                else
                {
                    targetConfiguration = Mapper.Map<TargetConfiguration>(request);
                }

                campaign.TargetConfiguration = targetConfiguration;

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
            throw new ValidationException(new ValidationError("", "Không có quyền cập nhật."));

        }
    }
}
