using AutoMapper;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Features.BlockTypes.Commands.CreateBlockType;
using IMP.Application.Features.BlockTypes.Commands.UpdateBlockType;
using IMP.Application.Features.Campaigns.Commands.CreateCampaign;
using IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType;
using IMP.Application.Features.CampaignTypes.Commands.UpdateCampaignType;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
using IMP.Application.Features.Platforms.Commands.UpdatePlatform;
using IMP.Domain.Entities;
using System.Collections.Generic;
using IMP.Application.Features.InfluencerPlatforms.Commands.CreateInfluencerPlatform;
using IMP.Application.Features.InfluencerPlatforms.Commands.UpdateInlfuencerPlatform;
using IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation;
using IMP.Application.Features.Pages.Commands.CreatePage;
using IMP.Application.Features.Pages.Commands.UpdatePage;
using IMP.Application.Features.Blocks.Commands.CreateBlock;
using IMP.Application.Features.Blocks.Commands.UpdateBlock;
using IMP.Application.Features.Vouchers.Commands.CreateVoucher;
using System.Linq;
using IMP.Application.Features.Brands.Commands.UpdateBrand;
using IMP.Application.Features.Vouchers.Commands.AssignVoucherForCampaign;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignInfluencerConfiguration;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignTargetConfiguration;
using Newtonsoft.Json;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaign;
using IMP.Application.Features.Campaigns.Commands.UpdateCampaignActivities;
using IMP.Application.Features.ActivityTypes.Commands.CreateActivityType;

namespace IMP.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region platform
            CreateMap<Platform, PlatformViewModel>().ReverseMap();
            CreateMap<CreatePlatformCommand, Platform>();
            CreateMap<UpdatePlatformCommand, Platform>();
            #endregion platform

            #region campaign type
            CreateMap<CampaignType, CampaignTypeViewModel>().ReverseMap();
            CreateMap<CreateCampaignTypeCommand, CampaignType>();
            CreateMap<UpdateCampaignTypeCommand, CampaignType>();
            #endregion campaign type

            #region campaign

            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<CampaignVoucherRequest, CampaignVoucher>();
            CreateMap<Campaign, CampaignViewModel>()
                .ForMember(dest => dest.Websites, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Website));
                  })
                .ForMember(dest => dest.Fanpages, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Fanpage));
                  })
                .ForMember(dest => dest.Hashtags, opt =>
                {
                    opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Hashtags));
                })
                .ForMember(dest => dest.Keywords, opt =>
                {
                    opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Keywords));
                })
                .ForMember(dest => dest.DefaultRewards, opt =>
                  {
                      opt.MapFrom(x => x.CampaignRewards.Where(c => c.IsDefaultReward == true).Select(x => new CampaignRewardViewModel
                      {
                          Name = x.Name,
                          Price = x.Price,
                          Currency = x.Currency
                      }));
                  })
                .ForMember(dest => dest.BestInfluencerRewards, opt =>
                {
                    opt.MapFrom(x => x.CampaignRewards.Where(c => c.IsDefaultReward == false).Select(x => new CampaignRewardViewModel
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Currency = x.Currency
                    }));
                });

            CreateMap<CampaignImage, CampaignImageViewModel>();
            CreateMap<CampaignImageRequest, CampaignImage>();
            CreateMap<CreateCampaignCommand, Campaign>();
            CreateMap<CampaignMilestone, CampaignMilestoneViewModel>();

            CreateMap<UpdateCampaignInformationCommand, Campaign>()
                .ForMember(dest => dest.Website, opt =>
                  {
                      opt.PreCondition(src => src.Websites != null);
                      opt.MapFrom(x => x.Websites == null ? "[]" : JsonConvert.SerializeObject(x.Websites));
                  })
                .ForMember(dest => dest.Fanpage, opt =>
                {
                    opt.PreCondition(src => src.Fanpages != null);
                    opt.MapFrom(x => x.Fanpages == null ? "[]" : JsonConvert.SerializeObject(x.Fanpages));
                })
                .ForMember(dest => dest.Hashtags, opt =>
                 {
                     opt.PreCondition(src => src.Hashtags != null);
                     opt.MapFrom(x => x.Hashtags == null ? "[]" : JsonConvert.SerializeObject(x.Hashtags));
                 })
                .ForMember(dest => dest.Keywords, opt =>
                {
                    opt.PreCondition(src => src.Websites != null);
                    opt.MapFrom(x => x.Websites == null ? "[]" : JsonConvert.SerializeObject(x.Keywords));
                })
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<LocationRequest, InfluencerConfigurationLocation>();
            CreateMap<LocationRequest, TargetConfigurationLocation>();

            CreateMap<UpdateInfluencerConfigurationCommand, InfluencerConfiguration>()
                .ForMember(dest => dest.OtherCondition, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.SerializeObject(x.Others));
                  })
                .ForMember(dest => dest.Jobs, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.SerializeObject(x.Jobs));
                  })
                .ForMember(dest => dest.Interests, opt =>
                {
                    opt.MapFrom(x => JsonConvert.SerializeObject(x.Interests));
                });

            CreateMap<UpdateTargetConfigurationCommand, TargetConfiguration>()
                .ForMember(dest => dest.Purpose, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.SerializeObject(x.Purposes));
                  })
                .ForMember(dest => dest.Jobs, opt =>
                {
                    opt.MapFrom(x => JsonConvert.SerializeObject(x.Jobs));
                })
                .ForMember(dest => dest.Interests, opt =>
                {
                    opt.MapFrom(x => JsonConvert.SerializeObject(x.Interests));
                });

            CreateMap<InfluencerConfiguration, InfluencerConfigurationViewModel>()
                .ForMember(dest => dest.Others, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.OtherCondition));
                  })
                .ForMember(dest => dest.Jobs, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Jobs));
                  })
                .ForMember(dest => dest.Interests, opt =>
                {
                    opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Interests));
                });

            CreateMap<InfluencerConfigurationLocation, InfluencerConfigurationLocationViewModel>();
            CreateMap<TargetConfiguration, TargetConfigurationViewModel>().ForMember(dest => dest.Purposes, opt =>
              {
                  opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Purpose));
              })
                .ForMember(dest => dest.Jobs, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Jobs));
                  })
                .ForMember(dest => dest.Interests, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<string>>(x.Interests));
                  });
            CreateMap<TargetConfigurationLocation, TargetConfigurationLocationViewModel>();

            // Campaign activity
            CreateMap<CampaignActivity, CampaignActivityViewModel>();
            CreateMap<ActivityResult, ActivityResultViewModel>();
            CreateMap<CampaignActivityUpdateModel, CampaignActivity>();
            CreateMap<ActivityResultUpdateModel, ActivityResult>();
            CreateMap<ActivityType, ActivityTypeViewModel>();
            CreateMap<CreateActivityTypeCommand, ActivityType>();
            #endregion

            #region milestone
            CreateMap<Milestone, MilestoneViewModel>();
            #endregion
            #region block type
            CreateMap<BlockType, BlockTypeViewModel>();
            CreateMap<CreateBlockTypeCommand, BlockType>();
            CreateMap<UpdateBlockTypeCommand, BlockType>();
            #endregion

            #region influencer platform
            CreateMap<InfluencerPlatform, InfluencerPlatformViewModel>()
                .ForMember(dest => dest.Interests, opt =>
                  {
                      opt.MapFrom(y => JsonConvert.DeserializeObject<List<string>>(y.Interests));
                  });
            CreateMap<CreateInfluencerPlatformCommand, InfluencerPlatform>()
                .ForMember(dest => dest.Interests, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.SerializeObject(x.Interests));
                  });
            CreateMap<UpdateInfluencerPlatformCommand, InfluencerPlatform>()
                .ForMember(dest => dest.Interests, opt =>
                {
                    opt.MapFrom(x => JsonConvert.SerializeObject(x.Interests));
                });
            #endregion

            #region application user
            CreateMap<ApplicationUser, ApplicationUserViewModel>();
            CreateMap<ApplicationUser, InfluencerViewModel>();
            CreateMap<UpdateUserInformationCommand, ApplicationUser>();
            CreateMap<PaymentInfor, PaymentInforViewModel>();
            CreateMap<Bank, BankViewModel>();
            #endregion

            #region brand
            CreateMap<Brand, BrandViewModel>().ForMember(dest => dest.JobB, opt =>
              {
                  opt.MapFrom(brand => CreateListJobFromJobStr(brand.Job));
              });

            CreateMap<UpdateBrandCommand, Brand>();
            #endregion brand

            #region location
            CreateMap<Location, LocationViewModel>();
            #endregion

            #region bio
            CreateMap<Page, PageViewModel>();
            CreateMap<CreatePageCommand, Page>();
            CreateMap<UpdatePageCommand, Page>();

            CreateMap<Block, BlockViewModel>()
                .ForMember(dest => dest.Data, opt =>
              {
                  opt.MapFrom(x => CreateDynamicObjectFromItems(x.Items));
              });
            CreateMap<CreateBlockCommand, Block>().ForMember(dest => dest.Items, opt =>
            {
                opt.MapFrom(x => x.Data.Properties().Select(x =>
                new BlockItem
                {
                    Key = x.Name,
                    Value = x.Value.ToString()
                }).ToList());
            });
            CreateMap<BlockRequest, Block>().ForMember(dest => dest.Items, opt =>
              {
                  opt.MapFrom(x => x.Data.Properties().Select(x =>
                  new BlockItem
                  {
                      Key = x.Name,
                      Value = x.Value.ToString()
                  }).ToList());
              });

            CreateMap<UpdateBlockCommand, Block>().ForMember(dest => dest.Items, opt =>
            {
                opt.MapFrom(x => x.Data.Properties().Select(x =>
                new BlockItem
                {
                    Key = x.Name,
                    Value = x.Value.ToString()
                }).ToList());
            });

            CreateMap<UpdateBlockRequest, Block>().ForMember(dest => dest.Items, opt =>
            {
                opt.MapFrom(x => x.Data.Properties().Select(x =>
                new BlockItem
                {
                    Key = x.Name,
                    Value = x.Value.ToString()
                }).ToList());
            });
            #endregion

            #region voucher
            CreateMap<Voucher, VoucherViewModel>();
            CreateMap<Voucher, UserVoucherViewModel>();
            CreateMap<CreateVoucherCommand, Voucher>();
            CreateMap<AssignVoucherToCampaignCommand, CampaignVoucher>();
            CreateMap<CampaignVoucher, CampaignVoucherViewModel>();
            CreateMap<CampaignVoucher, UserCampaignVoucherViewModel>();
            CreateMap<VoucherCode, VoucherCodeViewModel>();
            CreateMap<VoucherCode, UserVoucherCodeViewModel>();

            #endregion

            #region wallet
            CreateMap<Wallet, WalletViewModel>();
            CreateMap<WalletTransaction, WalletTransactionViewModel>().ForMember(x => x.Evidences, o =>
            {
                o.MapFrom(x => Newtonsoft.Json.JsonConvert.DeserializeObject<List<TransactionEvidence>>(x.Evidences));
            });
            CreateMap<ApplicationUser, TransactionUserViewModel>();
            #endregion

            #region ranking
            CreateMap<RankLevel, RankLevelViewModel>();
            CreateMap<Ranking, RankingViewModel>();
            #endregion
        }

        public IDictionary<string, string> CreateDynamicObjectFromItems(ICollection<BlockItem> items)
        {
            var result = new Dictionary<string, string>();
            if (items == null) return result;
            foreach (var item in items)
            {
                if (!result.ContainsKey(item.Key))
                    result.Add(item.Key, item.Value);
            }
            return result;
        }

        public List<string> CreateListJobFromJobStr(string job)
        {
            return job.Split(";").ToList();
        }

    }
}
