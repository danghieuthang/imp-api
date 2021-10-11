using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Pages.Commands.UpdatePage
{
    public class UpdatePageCommandValidator : AbstractValidator<UpdatePageCommand>
    {
        private readonly IGenericRepository<Page> _pageRepository;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;

        public UpdatePageCommandValidator(IUnitOfWork unitOfWork)
        {
            _pageRepository = unitOfWork.Repository<Page>();
            _applicationUserRepository = unitOfWork.Repository<ApplicationUser>();

            RuleFor(x => x.Id).MustAsync(async (x, y, z) =>
            {
                return await _pageRepository.IsExistAsync(x => x.Id == y && x.InfluencerId == x.InfluencerId);
            }).WithMessage("'{PropertyValue}' không tồn tại hoặc không có quyền chỉnh sửa.");
            RuleFor(x => x.InfluencerId).MustExistEntityId(async (x, y) =>
              {
                  return await _applicationUserRepository.IsExistAsync(x);
              });
            // RuleFor(x => x.BackgroundPhoto).MustValidUrl();
            // RuleFor(x => x.Title).MustRequired(256);
        }
    }
}