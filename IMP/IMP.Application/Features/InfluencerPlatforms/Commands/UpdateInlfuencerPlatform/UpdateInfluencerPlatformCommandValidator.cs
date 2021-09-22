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

namespace IMP.Application.Features.InfluencerPlatforms.Commands.UpdateInlfuencerPlatform
{
    public class UpdateInfluencerPlatformCommandValidator : AbstractValidator<UpdateInfluencerPlatformCommand>
    {
        private readonly IGenericRepository<InfluencerPlatform> _influencerPlatformRepositoryAsync;

        public UpdateInfluencerPlatformCommandValidator(IUnitOfWork unitOfWork)
        {
            _influencerPlatformRepositoryAsync = unitOfWork.Repository<InfluencerPlatform>();

            RuleFor(x => x.Url).MustValidUrl();
            RuleFor(x => x.PlatformId).MustAsync(async (x, y, z) =>
            {
                return await IsExistInfluencerPlatform(x.PlatformId, x.InfluencerId, z);
            }).WithMessage("'{PropertyValue}' Influencer Platform không tồn tại.");
        }


        public async Task<bool> IsExistInfluencerPlatform(int platformId, int influencerId, CancellationToken cancellationToken)
        {
            return await _influencerPlatformRepositoryAsync.FindSingleAsync(x => x.InfluencerId == influencerId && x.PlatformId == platformId) != null;
        }


    }
}
