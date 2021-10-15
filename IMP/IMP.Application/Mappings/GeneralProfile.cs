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
            CreateMap<Campaign, CampaignViewModel>();
            CreateMap<CampaignImage, CampaignImageViewModel>();
            CreateMap<CreateCampaignCommand, Campaign>();
            CreateMap<CampaignMilestone, CampaignMilestoneViewModel>();
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
            CreateMap<InfluencerPlatform, InfluencerPlatformViewModel>();
            CreateMap<CreateInfluencerPlatformCommand, InfluencerPlatform>();
            CreateMap<UpdateInfluencerPlatformCommand, InfluencerPlatform>();
            #endregion

            #region application user
            CreateMap<ApplicationUser, ApplicationUserViewModel>();
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
            CreateMap<CreateBlockCommand, Block>();
            CreateMap<BlockRequest, Block>().ForMember(dest => dest.Items, opt =>
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
            CreateMap<UpdateBlockCommand, Block>();
            #endregion

            #region voucher
            CreateMap<Voucher, VoucherViewModel>();
            CreateMap<CreateVoucherCommand, Voucher>();

            #endregion

            #region wallet
            CreateMap<Wallet, WalletViewModel>();
            CreateMap<WalletTransaction, WalletTransactionViewModel>().ForMember(x => x.Evidences, o =>
            {
                o.MapFrom(x => Newtonsoft.Json.JsonConvert.DeserializeObject<List<TransactionEvidence>>(x.Evidences));
            });
            CreateMap<ApplicationUser, TransactionUserViewModel>();
            #endregion
        }

        public IDictionary<string, string> CreateDynamicObjectFromItems(ICollection<BlockItem> items)
        {
            var result = new Dictionary<string, string>();
            if (items == null) return result;
            foreach (var item in items)
            {
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
