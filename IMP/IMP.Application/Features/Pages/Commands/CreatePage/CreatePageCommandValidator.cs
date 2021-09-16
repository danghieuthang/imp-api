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
        private readonly IGenericRepositoryAsync<ApplicationUser> _applicationUserRepositoryAsync;

        public CreatePageCommandValidator(IGenericRepositoryAsync<ApplicationUser> applicationUserRepositoryAsync, IOptions<FileSettings> settings)
        {
            _applicationUserRepositoryAsync = applicationUserRepositoryAsync;

            RuleFor(x => x.Title).MustRequired(256);
            RuleFor(x => x.BackgroundPhoto).MustValidUrl();
            RuleFor(x => x.InfluencerId).MustExistEntityId(IsExistUser);
        }

        private async Task<bool> IsExistUser(int id, CancellationToken cancellationToken)
        {
            return (await _applicationUserRepositoryAsync.GetByIdAsync(id)) != null;
        }

    }
}