using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Brands.Commands.ProcessBrand
{
    public class EnableBrandCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public class ApprovedBrandCommandHandler : CommandHandler<EnableBrandCommand, bool>
        {
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IGenericRepository<Brand> _brandRepository;
            public ApprovedBrandCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAuthenticatedUserService authenticatedUserService) : base(unitOfWork, mapper)
            {
                _authenticatedUserService = authenticatedUserService;
                _brandRepository = unitOfWork.Repository<Brand>();
            }

            public override async Task<Response<bool>> Handle(EnableBrandCommand request, CancellationToken cancellationToken)
            {
                //if (_authenticatedUserService.IsAdmin.Value)
                //{
                //    throw new ValidationException(new ValidationError("", "Không có quyền"));

                //}
                var brand = await _brandRepository.GetByIdAsync(request.Id);
                if (brand != null)
                {
                    brand.Status = (int)BrandStatus.Actived;
                    _brandRepository.Update(brand);
                    await UnitOfWork.CommitAsync();
                    return new Response<bool>(true);
                }
                throw new ValidationException(new ValidationError("id", "Nhãn hàng không tồn tại."));
            }
        }
    }
}
