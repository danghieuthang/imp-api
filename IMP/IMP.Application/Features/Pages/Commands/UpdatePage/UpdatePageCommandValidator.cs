using FluentValidation;
using IMP.Application.Constants;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Pages.Commands.UpdatePage
{
    public class UpdatePageCommandValidator : AbstractValidator<UpdatePageCommand>
    {
        private readonly IGenericRepository<Page> _pageRepository;
        private readonly IGenericRepository<Block> _blockRepository;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;

        public UpdatePageCommandValidator(IUnitOfWork unitOfWork)
        {
            _pageRepository = unitOfWork.Repository<Page>();
            _blockRepository = unitOfWork.Repository<Block>();
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

            RuleForEach(x => x.Blocks).MustAsync(async (x, y, z) =>
            {
                if (y.Id == 0) return true;
                return await _blockRepository.IsExistAsync(y.Id);
            }).WithMessage("'{PropertyValue}' không tồn tại.").WithErrorCode(errorCode: ErrorConstants.Application.Page.BlockIdNotValid.ToString());
        }
    }
}