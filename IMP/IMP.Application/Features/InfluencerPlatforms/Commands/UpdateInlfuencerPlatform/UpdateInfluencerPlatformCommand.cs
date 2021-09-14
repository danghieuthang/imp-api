using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.InfluencerPlatforms.Commands.UpdateInlfuencerPlatform
{
    public class UpdateInfluencerPlatformCommand : IRequest<Response<InfluencerPlatformViewModel>>
    {
        public int PlatformId { get; set; }
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public string Url { get; set; }

        public class UpdateInfluencerPlatformCommandHandler : IRequestHandler<UpdateInfluencerPlatformCommand, Response<InfluencerPlatformViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepositoryAsync<InfluencerPlatform> _influencerPlatformRepositoryAsync;

            public UpdateInfluencerPlatformCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _influencerPlatformRepositoryAsync = _unitOfWork.Repository<InfluencerPlatform>();
            }

            public async Task<Response<InfluencerPlatformViewModel>> Handle(UpdateInfluencerPlatformCommand request, CancellationToken cancellationToken)
            {
                var influencerPlatform = await _influencerPlatformRepositoryAsync.FindSingleAsync(x => x.InfluencerId == request.InfluencerId && x.PlatformId == request.PlatformId);

                influencerPlatform.Url = request.Url;

                _influencerPlatformRepositoryAsync.Update(influencerPlatform);
                await _unitOfWork.CommitAsync();

                var influencerPlatformView = new InfluencerPlatformViewModel
                {
                    Id = influencerPlatform.Id,
                    Url = influencerPlatform.Url,
                    //Influencer = new ApplicationUserViewModel
                    //{
                    //    Id = influencerPlatform.InfluencerId
                    //},
                    Platform = new PlatformViewModel
                    {
                        Id = influencerPlatform.PlatformId,
                    },
                    Created = influencerPlatform.Created,
                    LastModified = influencerPlatform.LastModified
                };

                return new Response<InfluencerPlatformViewModel>(influencerPlatformView);
            }
        }
    }
}
