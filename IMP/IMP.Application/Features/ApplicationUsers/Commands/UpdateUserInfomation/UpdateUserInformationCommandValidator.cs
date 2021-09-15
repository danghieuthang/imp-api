using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation
{
    public class UpdateUserInformationCommandValidator : AbstractValidator<UpdateUserInformationCommand>
    {
        private readonly IGenericRepositoryAsync<ApplicationUser> _applicationUserRepostoryAsync;
        private readonly IGenericRepositoryAsync<Location> _locationRepostoryAsync;

        public UpdateUserInformationCommandValidator(IUnitOfWork unitOfWork)
        {
            _applicationUserRepostoryAsync = unitOfWork.Repository<ApplicationUser>();
            _locationRepostoryAsync = unitOfWork.Repository<Location>();

            RuleFor(x => x.Id).IsExistId(IsExistUser);
            RuleFor(x => x.BirthDate).IsValidBirthDate();
            RuleFor(x => x.Description).MustMaxLength(2000);
            RuleFor(x => x.FirstName).MustMaxLength(256);
            RuleFor(x => x.LastName).MustMaxLength(256);
            RuleFor(x => x.InterestsR).ListMustContainFewerThan(10);
            RuleFor(x => x.PetR).ListMustContainFewerThan(10);
            RuleFor(x => x.JobR).ListMustContainFewerThan(10);
            RuleFor(x => x.LocationId).MustAsync(async (x, y) =>
            {
                if (x == null)
                {
                    return true;
                }
                return await IsExistLocation(x.Value, y);
            }).WithMessage("Địa chỉ không hợp lệ.");

        }

        public async Task<bool> IsExistUser(int id, CancellationToken cancellationToken)
        {
            return (await _applicationUserRepostoryAsync.GetByIdAsync(id)) != null;
        }

        public async Task<bool> IsExistLocation(int id, CancellationToken cancellationToken)
        {
            return (await _locationRepostoryAsync.GetByIdAsync(id)) != null;
        }
    }
}
