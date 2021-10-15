using AutoMapper;
using IMP.Application.Features.Brands.Commands.UpdateBrand;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : CreateBrandRequest, ICommand<BrandViewModel>
    {
        [JsonIgnore]
        public int ApplicationUserId { get; set; }
        public class CreateBrandCommandHandler : CommandHandler<CreateBrandCommand, BrandViewModel>
        {
            private IGenericRepository<Brand> _brandRepository;
            public CreateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _brandRepository = unitOfWork.Repository<Brand>();
            }


            public override async Task<Response<BrandViewModel>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.Repository<ApplicationUser>().FindSingleAsync(x => x.Id == request.ApplicationUserId, includeProperties: x => x.Brand);

                if (user?.Brand == null)
                {
                    var brand = Mapper.Map<Brand>(request);
                    brand.Job = string.Join(";", request.JobB);
                    await UnitOfWork.CommitAsync();
                    var brandView = Mapper.Map<BrandViewModel>(brand);
                    return new Response<BrandViewModel>(brandView);
                }
                return new Response<BrandViewModel>(error: new Models.ValidationError("id", "Người dùng đã là nhãn hàng."));
            }


        }

    }
}
