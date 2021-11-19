using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandInformationCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandInformationCommandValidator(IUnitOfWork unitOfWork)
        {
            var brandRepository = unitOfWork.Repository<Brand>();
            RuleFor(x => x.Code).MustMaxLength(256);
            RuleFor(x => x.CompanyName).MustMaxLength(256);
            RuleFor(x => x.Image).MustValidUrl(allowNull: true);
            RuleFor(x => x.Address).MustMaxLength(256);
            RuleFor(x => x.Fanpage).MustMaxLength(256);
            RuleFor(x => x.Website).MustValidUrl(allowNull: true);
            RuleFor(x => x.Representative).MustMaxLength(256);
            RuleFor(x => x.Phone).MustValidPhoneNumber(allowNull: true);
            RuleFor(x => x.JobB).ListMustContainFewerThan(10);
            RuleFor(x => x.Introduction).MustMaxLength(2000);

            RuleFor(x => x.Email).MustValidEmail().MustAsync(async (x, y, z) =>
            {
                var brand = await brandRepository.GetByIdAsync(x.Id);
                if (brand.Email == y)
                {
                    return true;
                }
                return (await brandRepository.FindSingleAsync(user => user.Email == y && user.Id != x.Id)) == null;
            }).WithMessage("Email đã tồn tại hoặc không thể cập nhật email.");

        }
    }
}
