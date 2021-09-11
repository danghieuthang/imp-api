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
using IMP.Application.Features.Products.Commands.CreateProduct;
using IMP.Application.Features.Products.Queries.GetAllProducts;
using IMP.Application.Models.ViewModels;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using IMP.Application.Features.InfluencerPlatforms.Commands.CreateInfluencerPlatform;
using IMP.Application.Features.InfluencerPlatforms.Commands.UpdateInlfuencerPlatform;

namespace IMP.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();

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
            CreateMap<CreateCampaignCommand, Campaign>();
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
            #endregion

            #region 
            #endregion

        }
    }
}
