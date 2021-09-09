using AutoMapper;
using IMP.Application.DTOs.Compaign;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Features.Campaigns.Commands.CreateCampaign;
using IMP.Application.Features.CampaignTypes.Commands.CreateCampaignType;
using IMP.Application.Features.CampaignTypes.Commands.UpdateCampaignType;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
using IMP.Application.Features.Platforms.Commands.UpdatePlatform;
using IMP.Application.Features.Products.Commands.CreateProduct;
using IMP.Application.Features.Products.Queries.GetAllProducts;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

        }
    }
}
