using AutoMapper;
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
        public int ApplicationUserId { get; set; }
        public class GetBrandByApplicationUserIdQueryHandler : QueryHandler<GetBrandByApplicationUserIdQuery, BrandViewModel>
        {
            public GetBrandByApplicationUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<BrandViewModel>> Handle(GetBrandByApplicationUserIdQuery request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == request.ApplicationUserId,
                        include: x => x.Include(y => y.Brand));
                if (user?.Brand != null)
                {
                    var brandView = Mapper.Map<BrandViewModel>(user.Brand);
                    return new Response<BrandViewModel>(brandView);
                }
                throw new KeyNotFoundException(message: "Không tồn tại.");
            }
        }
    }
}
