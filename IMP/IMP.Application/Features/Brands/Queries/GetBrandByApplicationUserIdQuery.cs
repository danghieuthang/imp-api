﻿using AutoMapper;
using IMP.Application.Interfaces;
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

namespace IMP.Application.Features.Brands.Queries
{
    public class GetBrandByApplicationUserIdQuery : IQuery<BrandViewModel>
    {
        public class GetBrandByApplicationUserIdQueryHandler : QueryHandler<GetBrandByApplicationUserIdQuery, BrandViewModel>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public GetBrandByApplicationUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<BrandViewModel>> Handle(GetBrandByApplicationUserIdQuery request, CancellationToken cancellationToken)
            {
                var brand = await UnitOfWork.Repository<Brand>().FindSingleAsync(x => x.Id == _authenticatedUserService.BrandId);
                if (brand != null)
                {
                    var brandView = Mapper.Map<BrandViewModel>(brand);
                    return new Response<BrandViewModel>(brandView);
                }
                throw new KeyNotFoundException(message: "Không tồn tại.");
            }
        }
    }
}
