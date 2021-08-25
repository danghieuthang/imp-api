using AutoMapper;
using IMP.Application.DTOs.ViewModels;
using IMP.Application.Features.Platforms.Commands.CreatePlatform;
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
            #endregion platform
        }
    }
}
