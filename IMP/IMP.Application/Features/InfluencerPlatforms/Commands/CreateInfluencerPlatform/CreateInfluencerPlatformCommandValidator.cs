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
        private readonly IGenericRepository<InfluencerPlatform> _influencerPlatformRepository;
        private readonly IGenericRepository<ApplicationUser> _applicationUserRepository;
        private readonly IGenericRepository<Platform> _platformRepository;
        public CreateInfluencerPlatformCommandValidator(IUnitOfWork unitOfWork)
        {
            _influencerPlatformRepository = unitOfWork.Repository<InfluencerPlatform>();
            _applicationUserRepository = unitOfWork.Repository<ApplicationUser>();
            _platformRepository = unitOfWork.Repository<Platform>();

            RuleFor(x => x.Url).MustValidUrl(allowNull: false);
            RuleFor(x => x.InfluencerId).MustExistEntityId(IsValidInfluencerId);
            RuleFor(x => x.PlatformId).MustExistEntityId(IsValidPlatform);
            RuleFor(x => x.PlatformId).MustAsync(async (x, y, z) =>
              {
                  return await IsDuplicatePlatform(y, x.InfluencerId, z);
              }).WithMessage("'{PropertyValue}' đã có trên tài khoản(Mỗi tài khoản chỉ có 1 url platform).");

            RuleFor(x => x.Interests).NotNull().WithMessage("Danh sách sở thích chưa có.").Must(x => x.Count() > 0).WithMessage("Danh sách sở thích chưa có.");
        }

        public async Task<bool> IsValidInfluencerId(int id, CancellationToken cancellationToken)
        {
            return await _applicationUserRepository.GetByIdAsync(id) != null;
        }

        public async Task<bool> IsValidPlatform(int id, CancellationToken cancellationToken)
        {
            return await _platformRepository.GetByIdAsync(id) != null;
        }

        public async Task<bool> IsDuplicatePlatform(int platformId, int influencerId, CancellationToken cancellationToken)
        {
            return await _influencerPlatformRepository.FindSingleAsync(x => x.PlatformId == platformId && x.InfluencerId == influencerId) == null;
        }
    }
}
