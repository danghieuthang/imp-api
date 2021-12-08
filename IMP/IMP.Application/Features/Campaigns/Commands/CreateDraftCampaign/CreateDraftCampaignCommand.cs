using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Campaigns.Commands.CreateDraftCampaign
{
    public class CreateDraftCampaignCommand : ICommand<CampaignViewModel>
    {
        public class CreateDraftCampaignCommandHandler : CommandHandler<CreateDraftCampaignCommand, CampaignViewModel>
        {
            private readonly IGenericRepository<Campaign> _campaignRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public CreateDraftCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _campaignRepository = unitOfWork.Repository<Campaign>();
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<CampaignViewModel>> Handle(CreateDraftCampaignCommand request, CancellationToken cancellationToken)
            {
                var applicationUser = await UnitOfWork.Repository<ApplicationUser>().GetByIdAsync(_authenticatedUserService.ApplicationUserId);
                var campaign = new Campaign
                {
                    CreatedById = applicationUser.Id,
                    BrandId = applicationUser.BrandId.Value,
                    Status = (int)CampaignStatus.Draft,
                    IsActived = true,
                    OpeningDate = DateTime.Now,
                    ApplyingDate = DateTime.Now,
                    AdvertisingDate = DateTime.Now,
                    EvaluatingDate = DateTime.Now,
                    AnnouncingDate = DateTime.Now,
                    ClosedDate = DateTime.Now,
                    Description = string.Empty,
                    Title = string.Empty,
                    AdditionalInformation = string.Empty,
                    ProductInformation = string.Empty,
                    SampleContent = string.Empty,
                    InfluencerConfiguration = new InfluencerConfiguration
                    {
                        LevelId = 1,
                        Gender = (int)Genders.None,
                        ChildStatus = null,
                        MaritalStatus = null,
                        Pregnant = null,
                        UnlimitedAge = true,
                        AgeFrom = 18,
                        AgeTo = 25,
                        PlatformId = 1
                    },
                    TargetConfiguration = new TargetConfiguration
                    {
                        Gender = (int)Genders.None,
                        ChildStatus = null,
                        MaritalStatus = null,
                        Pregnant = null,
                        UnlimitedAge = true,
                        AgeFrom = 18,
                        AgeTo = 25
                    },
                    VoucherCommissionMode = (int)VoucherCommissionType.Order,
                    IsPercentVoucherCommission = false,
                    VoucherCommissionPrices = "[]"
                };

                await _campaignRepository.AddAsync(campaign);
                await UnitOfWork.CommitAsync();

                var campaignView = Mapper.Map<CampaignViewModel>(campaign);
                campaignView.InfluencerConfiguration.Platform = new Models.ViewModels.PlatformViewModel
                {
                    Id = campaign.InfluencerConfiguration.PlatformId.Value
                };
                return new Response<CampaignViewModel>(campaignView);
            }
        }
    }
}
