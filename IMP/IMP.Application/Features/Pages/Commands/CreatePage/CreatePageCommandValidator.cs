using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
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

        public CreatePageCommandValidator(IUnitOfWork  unitOfWork, IOptions<FileSettings> settings)
        {
            _applicationUserRepository = unitOfWork.Repository<ApplicationUser>();

            RuleFor(x => x.BackgroundType).MustRequired(256);
            RuleFor(x => x.InfluencerId).MustExistEntityId(IsExistUser);
        }

        private async Task<bool> IsExistUser(int id, CancellationToken cancellationToken)
        {
            return await _applicationUserRepository.IsExistAsync(id);
        }

    }
}