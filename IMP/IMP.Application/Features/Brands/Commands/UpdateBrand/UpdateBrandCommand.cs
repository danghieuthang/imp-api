using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
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

namespace IMP.Application.Features.Brands.Commands.UpdateBrand
{
    public class CreateBrandRequest
    {
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Image { get; set; }
        public bool IsCompany { get; set; }
        public string Website { get; set; }
        public string Introduction { get; set; }
        public string Representative { get; set; }
        public string Fanpage { get; set; }
        [JsonProperty("job")]
        public List<string> JobB { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }

    public class UpdateBrandCommand : CreateBrandRequest, ICommand<BrandViewModel>
    {
        public int Id { get; set; }
        public class UpdateBrandCommandHandler : CommandHandler<UpdateBrandCommand, BrandViewModel>
        {
            private readonly IGenericRepository<Brand> _brandRepository;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            public UpdateBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _brandRepository = unitOfWork.Repository<Brand>();
                _authenticatedUserService = authenticatedUserService;
            }

            public override async Task<Response<BrandViewModel>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
            {
                var brand = await _brandRepository.GetByIdAsync(request.Id);
                if (brand != null)
                {
                    if (brand.Id != _authenticatedUserService.BrandId)
                    {
                        throw new ValidationException(new ValidationError("", "Không có quyền cập nhật nhãn hàng này"));
                    }

                    Mapper.Map(request, brand);
                    _brandRepository.Update(brand);

                    brand.Job = string.Join(";", request.JobB);

                    await UnitOfWork.CommitAsync();

                    var brandView = Mapper.Map<BrandViewModel>(brand);
                    return new Response<BrandViewModel>(brandView);
                }
                return new Response<BrandViewModel>(error: new Models.ValidationError("id", "Brand không tồn tại."));

            }
        }
    }
}
