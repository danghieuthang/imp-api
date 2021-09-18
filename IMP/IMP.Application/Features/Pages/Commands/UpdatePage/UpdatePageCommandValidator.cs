using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Pages.Commands.UpdatePage
{
    public class UpdatePageCommandValidator : AbstractValidator<UpdatePageCommand>
    {
        private readonly IGenericRepositoryAsync<Page> _pageRepositoryAsync;
        private readonly IGenericRepositoryAsync<ApplicationUser> _applicationUserRepositoryAsync;

        public UpdatePageCommandValidator(IUnitOfWork unitOfWork)
        {
            _pageRepositoryAsync = unitOfWork.Repository<Page>();
            _applicationUserRepositoryAsync = unitOfWork.Repository<ApplicationUser>();

            RuleFor(x => x.Id).MustAsync(async (x, y, z) =>
            {
                return await _pageRepositoryAsync.IsExistAsync(x => x.Id == y && x.InfluencerId == x.InfluencerId);
            }).WithMessage("'{PropertyValue}' không tồn tại hoặc không có quyền chỉnh sửa.");
            RuleFor(x => x.InfluencerId).MustExistEntityId(async (x, y) =>
              {
                  return await _applicationUserRepositoryAsync.IsExistAsync(x);
              });
            RuleFor(x => x.BackgroundPhoto).MustValidUrl();
            RuleFor(x => x.Title).MustRequired(256);
        }
    }
}