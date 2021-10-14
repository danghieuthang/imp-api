using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IMP.Application.Constants;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Application.Validations;
using IMP.Domain.Entities;
using IMP.Domain.Settings;
using Microsoft.Extensions.Options;

namespace IMP.Application.Features.Pages.Commands.CreatePage
{
    public class CreatePageCommandValidator : AbstractValidator<CreatePageCommand>
    {
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;
        private readonly IGenericRepository<Page> _pageRepository;

        public CreatePageCommandValidator(IUnitOfWork unitOfWork, IOptions<FileSettings> settings)
        {
            _applicationUserRepository = unitOfWork.Repository<ApplicationUser>();
            _pageRepository = unitOfWork.Repository<Page>();

            RuleFor(x => x.BackgroundType).MustRequired(256);
            RuleFor(x => x.InfluencerId).MustExistEntityId(IsExistUser);
            RuleFor(x => x.BioLink).MustValidNickname(message: "Không hợp lệ").WithErrorCode(errorCode: ErrorConstants.Application.Page.BioLinkNotValid.ToString())
                .MustAsync(async (x, y, z) =>
            {
                if (string.IsNullOrEmpty(y))
                {
                    return true;
                }
                return !await _pageRepository.IsExistAsync(predicate: page => page.BioLink == y);
            }).WithMessage("Đã tồn tại.").WithErrorCode(errorCode: ErrorConstants.Application.Page.BioLinkDuplicate.ToString());
        }

        private async Task<bool> IsExistUser(int id, CancellationToken cancellationToken)
        {
            return await _applicationUserRepository.IsExistAsync(id);
        }

    }
}