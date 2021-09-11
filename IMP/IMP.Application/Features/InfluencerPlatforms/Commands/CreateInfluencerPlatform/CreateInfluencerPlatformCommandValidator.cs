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

namespace IMP.Application.Features.InfluencerPlatforms.Commands.CreateInfluencerPlatform
{
    public class CreateInfluencerPlatformCommandValidator : AbstractValidator<CreateInfluencerPlatformCommand>
    {
        private readonly IGenericRepositoryAsync<int, InfluencerPlatform> _influencerPlatformRepositoryAsync;
        private readonly IGenericRepositoryAsync<int, ApplicationUser> _applicationUserRepositoryAsync;
        private readonly IGenericRepositoryAsync<int, Platform> _platformRepositoryAsync;
        public CreateInfluencerPlatformCommandValidator(IGenericRepositoryAsync<int, InfluencerPlatform> influencerPlatformRepositoryAsync, IGenericRepositoryAsync<int, ApplicationUser> applicationUserRepositoryAsync, IGenericRepositoryAsync<int, Platform> platformRepositoryAsync)
        {
            _influencerPlatformRepositoryAsync = influencerPlatformRepositoryAsync;
            _applicationUserRepositoryAsync = applicationUserRepositoryAsync;
            _platformRepositoryAsync = platformRepositoryAsync;


            RuleFor(x => x.Url).IsValidUrl();
            RuleFor(x => x.InfluencerId).IsExistId(IsValidInfluencerId);
            RuleFor(x => x.PlatformId).IsExistId(IsValidPlatform);
            RuleFor(x => x.PlatformId).MustAsync(async (x, y, z) =>
              {
                  return await IsDuplicatePlatform(y, x.InfluencerId, z);
              }).WithMessage("'{PropertyValue}' đã có trên tài khoản(Mỗi tài khoản chỉ có 1 url platform).");
        }

        public async Task<bool> IsValidInfluencerId(int id, CancellationToken cancellationToken)
        {
            return await _applicationUserRepositoryAsync.GetByIdAsync(id) != null;
        }

        public async Task<bool> IsValidPlatform(int id, CancellationToken cancellationToken)
        {
            return await _platformRepositoryAsync.GetByIdAsync(id) != null;
        }

        public async Task<bool> IsDuplicatePlatform(int platformId, int influencerId, CancellationToken cancellationToken)
        {
            return await _influencerPlatformRepositoryAsync.FindSingleAsync(x => x.PlatformId == platformId && x.InfluencerId == influencerId) == null;
        }
    }
}
