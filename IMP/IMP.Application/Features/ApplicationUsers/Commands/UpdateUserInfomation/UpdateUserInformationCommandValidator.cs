using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.ApplicationUsers.Commands.UpdateUserInfomation
{
    public class UpdateUserInformationCommandValidator : AbstractValidator<UpdateUserInformationCommand>
    {
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepostoryAsync;
        private readonly IGenericRepository<Location> _locationRepostoryAsync;

        public UpdateUserInformationCommandValidator(IUnitOfWork unitOfWork)
        {
            _applicationUserRepostoryAsync = unitOfWork.Repository<ApplicationUser>();
            _locationRepostoryAsync = unitOfWork.Repository<Location>();
            RuleFor(x => x.Id).MustExistEntityId(IsExistUser);
            RuleFor(x=>x.Avatar).MustValidUrl(true);
            RuleFor(x => x.BirthDate).MustValidBirthDate();
            RuleFor(x => x.Description).MustMaxLength(2000);
            RuleFor(x => x.FirstName).MustMaxLength(256);
            RuleFor(x => x.LastName).MustMaxLength(256);
            RuleFor(x => x.InterestsR).ListMustContainFewerThan(10);
            RuleFor(x => x.PetR).ListMustContainFewerThan(10);
            RuleFor(x => x.JobR).ListMustContainFewerThan(10);
            RuleFor(x => x.Email).MustAsync(async (x, y, z) =>
            {
                var user = await _applicationUserRepostoryAsync.GetByIdAsync(x.Id);
                if(user.Email==y){
                    return true;
                }
                if (!user.IsEmailVerified)
                {
                    return (await _applicationUserRepostoryAsync.FindSingleAsync(user => user.Email == y && user.Id!=x.Id)) == null;
                }
                return false;
            }).WithMessage("Email đã tồn tại hoặc không thể cập nhật email.");
            RuleFor(x => x.LocationId).MustAsync(async (x, y) =>
            {
                if (x == null)
                {
                    return true;
                }
                return await IsExistLocation(x.Value, y);
            }).WithMessage("Địa chỉ không hợp lệ.");
            RuleFor(x => x.PhoneNumber).MustValidPhoneNumber(true);
            RuleFor(x => x.Nickname).MustValidNickname(allowNull: true)
                .MustAsync(async (x, y, z) =>
              {
                  return await IsValidNickname(x.Id, y, z);
              }).WithMessage(@"Nickname đã tồn tại.");


        }

        public async Task<bool> IsExistUser(int id, CancellationToken cancellationToken)
        {
            return (await _applicationUserRepostoryAsync.GetByIdAsync(id)) != null;
        }

        public async Task<bool> IsExistLocation(int id, CancellationToken cancellationToken)
        {
            return (await _locationRepostoryAsync.GetByIdAsync(id)) != null;
        }

        public async Task<bool> IsValidNickname(int id, string nickname, CancellationToken cancellationToken)
        {
            return (await _applicationUserRepostoryAsync.FindSingleAsync(x => x.Nickname.ToLower() == nickname.ToLower() && x.Id != id)) == null;

        }
    }
}
