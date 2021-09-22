using System.Security.Cryptography;
using AutoMapper;
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
using IMP.Application.Helpers;

namespace IMP.Application.Features.InfluencerPlatforms.Commands.CreateInfluencerPlatform
{
    public class CreateInfluencerPlatformCommand : IRequest<Response<InfluencerPlatformViewModel>>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int PlatformId { get; set; }
        public string Url { get; set; }

        public class CreateInfluencerPlatformCommandHandler : IRequestHandler<CreateInfluencerPlatformCommand, Response<InfluencerPlatformViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<InfluencerPlatform> _influencerPlatformRepositoryAsync;
            private readonly IMapper _mapper;

            public CreateInfluencerPlatformCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _influencerPlatformRepositoryAsync = _unitOfWork.Repository<InfluencerPlatform>();
                _mapper = mapper;
            }

            public async Task<Response<InfluencerPlatformViewModel>> Handle(CreateInfluencerPlatformCommand request, CancellationToken cancellationToken)
            {
                var influencerPlatform = _mapper.Map<InfluencerPlatform>(request);
                influencerPlatform.Hashtag="imp_"+StringHelper.RandomString(10);
                influencerPlatform = await _influencerPlatformRepositoryAsync.AddAsync(influencerPlatform);
                await _unitOfWork.CommitAsync();

                var influencerPlatformView = _mapper.Map<InfluencerPlatformViewModel>(influencerPlatform);
                influencerPlatformView.Influencer = new ApplicationUserViewModel { Id = request.InfluencerId };
                influencerPlatformView.Platform = new PlatformViewModel { Id = request.PlatformId };

                return new Response<InfluencerPlatformViewModel>(influencerPlatformView);
            }
        }
    }
}
