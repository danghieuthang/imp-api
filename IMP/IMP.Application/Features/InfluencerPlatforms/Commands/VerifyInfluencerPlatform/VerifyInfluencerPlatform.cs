using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using IMP.Domain.SocialPlatforms;
using MediatR;
using Newtonsoft.Json;

namespace IMP.Application.Features.InfluencerPlatforms.Commands.RequestVerifyInfluencerPlatform
{
    public class VerifyInfluencerPlatformCommand : ICommand<InfluencerPlatformViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public class VerifyInfluencerPlatformCommandHandler : CommandHandler<VerifyInfluencerPlatformCommand, InfluencerPlatformViewModel>
        {
            private readonly IGenericRepositoryAsync<InfluencerPlatform> _influencerPlatformRepositoryAsync;
            private readonly IMapper _mapper;
            private readonly ITiktokService _tiktokService;
            private readonly IFacebookService _facebookService;

            public VerifyInfluencerPlatformCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITiktokService tiktokService, IFacebookService facebookService) : base(unitOfWork)
            {
                _influencerPlatformRepositoryAsync = unitOfWork.Repository<InfluencerPlatform>();
                _mapper = mapper;
                _tiktokService = tiktokService;
                _facebookService = facebookService;
            }

            public override async Task<Response<InfluencerPlatformViewModel>> Handle(VerifyInfluencerPlatformCommand request, CancellationToken cancellationToken)
            {
                var influencerPlatform = await _influencerPlatformRepositoryAsync.FindSingleAsync(x => x.Id == request.Id, x => x.Platform);
                if (influencerPlatform == null)
                {
                    throw new ValidationException(new ValidationError("id", $"Không tồn tại."));
                }
                if (influencerPlatform.InfluencerId != request.InfluencerId)
                {
                    throw new ValidationException(new ValidationError("id", $"Không có quyền xác thực."));
                }

                SocialPlatformUser socialPlatformUser = null;
                // If titok platform
                if (influencerPlatform.Platform.Name.Equals("Tiktok", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    socialPlatformUser = await _tiktokService.VerifyUser(username: influencerPlatform.Url, hashtag: influencerPlatform.Hashtag);
                }
                // If facebook platform
                if (influencerPlatform.Platform.Name.Equals("Facebook", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    socialPlatformUser = await _facebookService.VerifyUser(username: influencerPlatform.Url, accessToken: request.AccessToken);
                }

                if (socialPlatformUser == null)
                {
                    return new Response<InfluencerPlatformViewModel>
                    {
                        Succeeded = false,
                        Message = "Xác thực không thành công."
                    };
                }

                influencerPlatform.Avatar = socialPlatformUser.Avatar;
                influencerPlatform.Follower = socialPlatformUser.Follower;
                influencerPlatform.IsVerified = true;

                _influencerPlatformRepositoryAsync.Update(influencerPlatform);
                await UnitOfWork.CommitAsync();

                var influencerPlatformView = _mapper.Map<InfluencerPlatformViewModel>(influencerPlatform);
                return new Response<InfluencerPlatformViewModel>(influencerPlatformView);
            }

        }
    }
}