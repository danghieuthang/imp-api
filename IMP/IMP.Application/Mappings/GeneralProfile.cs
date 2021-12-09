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
using IMP.Application.Features.Vouchers.Commands.UpdateVoucher;
using System;
using IMP.Domain.Common;
using IMP.Application.Models;
using IMP.Application.Features.VoucherTransactions.Commands.CreateVoucherTransaction;

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
                })
                 .ForMember(dest => dest.DefaultVoucherRewards, opt =>
                 {
                     opt.MapFrom(x => x.Vouchers.Where(c => c.IsDefaultReward == true));
                 })
                 .ForMember(dest => dest.BestInfluencerVoucherRewards, opt =>
                  {
                      opt.MapFrom(x => x.Vouchers.Where(c => c.IsBestInfluencerReward == true));
                  })
                 .ForMember(dest => dest.Vouchers, opt =>
                 {
                     opt.MapFrom(x => x.Vouchers.Where(c => c.IsDefaultReward == false && c.IsBestInfluencerReward == false));
                 })
                 .ForMember(dest => dest.VoucherCommissionPrices, opt =>
                   {
                       opt.MapFrom(x => JsonConvert.DeserializeObject<List<VoucherCommissionPrices>>(x.VoucherCommissionPrices));
                   });

            CreateMap<CampaignImage, CampaignImageViewModel>();
            CreateMap<CampaignImageRequest, CampaignImage>();
            CreateMap<CreateCampaignCommand, Campaign>();

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
                    opt.PreCondition(src => src.Keywords != null);
                    opt.MapFrom(x => x.Keywords == null ? "[]" : JsonConvert.SerializeObject(x.Keywords));
                })
                 .ForMember(dest => dest.VoucherCommissionPrices, opt =>
                 {
                     opt.PreCondition(src => src.VoucherCommissionPrices != null);
                     opt.MapFrom(x => x.VoucherCommissionPrices == null ? "[]" : JsonConvert.SerializeObject(x.VoucherCommissionPrices));
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
            CreateMap<CampaignActivity, CampaignActivityBasicViewModel>();
            CreateMap<CampaignActivityUpdateModel, CampaignActivity>();
            CreateMap<ActivityType, ActivityTypeViewModel>();
            CreateMap<CreateActivityTypeCommand, ActivityType>();
            CreateMap<Campaign, CampaignBasicInfoViewModel>();
            CreateMap<ActivityType, ActivityTypeBasicInfoViewModel>();

            #endregion

            #region milestone
            //CreateMap<Milestone, MilestoneViewModel>();
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
            CreateMap<ApplicationUser, UserBasicViewModel>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>()
                .ForMember(dest => dest.InterestsR, opt =>
                  {
                      opt.MapFrom(x => CreateListJobFromJobStr(x.Interests));
                  })
                .ForMember(dest => dest.JobsR, opt =>
                {
                    opt.MapFrom(x => CreateListJobFromJobStr(x.Job));
                })
                .ForMember(dest => dest.PetsR, opt =>
                {
                    opt.MapFrom(x => CreateListJobFromJobStr(x.Pet));
                });
            CreateMap<ApplicationUser, InfluencerViewModel>()
                 .ForMember(dest => dest.InterestsR, opt =>
                 {
                     opt.MapFrom(x => CreateListJobFromJobStr(x.Interests));
                 })
                .ForMember(dest => dest.JobsR, opt =>
                {
                    opt.MapFrom(x => CreateListJobFromJobStr(x.Job));
                })
                .ForMember(dest => dest.PetsR, opt =>
                {
                    opt.MapFrom(x => CreateListJobFromJobStr(x.Pet));
                });
            CreateMap<UpdateUserInformationCommand, ApplicationUser>();
            CreateMap<PaymentInfor, PaymentInforViewModel>();
            CreateMap<Bank, BankViewModel>();
            CreateMap<ApplicationUser, AdminViewModel>();
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
            CreateMap<Voucher, VoucherViewModel>()
                .ForMember(dest => dest.DiscountProducts, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<DiscountProductViewModel>>(x.DiscountProducts));
                  });
            CreateMap<Voucher, UserVoucherViewModel>()
                  .ForMember(dest => dest.DiscountProducts, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<DiscountProductViewModel>>(x.DiscountProducts));
                  });
            CreateMap<CreateVoucherCommand, Voucher>()
                .ForMember(x => x.DiscountProducts, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.SerializeObject(x.DiscountProducts));
                  });
            CreateMap<UpdateVoucherCommand, Voucher>()
               .ForMember(x => x.DiscountProducts, opt =>
               {
                   opt.MapFrom(x => JsonConvert.SerializeObject(x.DiscountProducts));
               });

            CreateMap<Voucher, VoucherReportViewModel>()
                .ForMember(dest => dest.DiscountProducts, opt =>
                {
                    opt.MapFrom(x => JsonConvert.DeserializeObject<List<DiscountProductViewModel>>(x.DiscountProducts));
                });
            CreateMap<Voucher, UserVoucherViewModel>()
                  .ForMember(dest => dest.DiscountProducts, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.DeserializeObject<List<DiscountProductViewModel>>(x.DiscountProducts));
                  });
            CreateMap<CreateVoucherCommand, Voucher>()
                .ForMember(x => x.DiscountProducts, opt =>
                {
                    opt.MapFrom(x => JsonConvert.SerializeObject(x.DiscountProducts));
                });
            CreateMap<UpdateVoucherCommand, Voucher>()
               .ForMember(x => x.DiscountProducts, opt =>
               {
                   opt.MapFrom(x => JsonConvert.SerializeObject(x.DiscountProducts));
               });
            CreateMap<AssignVoucherToCampaignCommand, CampaignVoucher>();
            CreateMap<CampaignVoucher, CampaignVoucherViewModel>();
            CreateMap<CampaignVoucher, UserCampaignVoucherViewModel>();
            CreateMap<CampaignVoucher, CampaignVoucherOnlyIdViewModel>();
            CreateMap<VoucherCode, VoucherCodeViewModel>();
            CreateMap<VoucherCode, UserVoucherCodeViewModel>();

            CreateMap<VoucherTransaction, VoucherTransactionViewModel>()
                .ForMember(dest => dest.Order, opt =>
                  {
                      opt.PreCondition(x => x.Order != null);
                      opt.MapFrom(x => JsonConvert.DeserializeObject<Order>(x.Order));
                  });
            CreateMap<CreateVoucherTransactionCommand, VoucherTransaction>()
                .ForMember(dest => dest.Order, opt =>
                  {
                      opt.MapFrom(x => JsonConvert.SerializeObject(x.Order));
                  });
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

            #region campaign member
            CreateMap<CampaignMember, CampaignMemberViewModel>();
            CreateMap<CampaignMember, CampaignMemberBasicInfoViewModel>();
            #endregion

            #region evidence type
            CreateMap<EvidenceType, EvidenceTypeViewModel>();
            #endregion

            #region evidence
            CreateMap<Evidence, EvidenceViewModel>();
            #endregion

            #region member activity
            CreateMap<MemberActivity, MemberActivityViewModel>();
            CreateMap<ActivityComment, ActivityCommentViewModel>();
            #endregion



            CreateMap<Notification, NotificationViewModel>();
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
            if (job == null) return new List<string>();
            return job.Split(";").ToList();
        }

        public DateTime? GetUtcDate(DateTime? date)
        {
            if (date == null) return null;
            return DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
        }

    }
}
