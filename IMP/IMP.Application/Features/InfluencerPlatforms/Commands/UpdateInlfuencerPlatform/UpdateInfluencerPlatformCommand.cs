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

namespace IMP.Application.Features.InfluencerPlatforms.Commands.UpdateInlfuencerPlatform
{
    public class UpdateInfluencerPlatformCommand : ICommand<InfluencerPlatformViewModel>
    {
        public int PlatformId { get; set; }
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public string Url { get; set; }
        public List<string> Interests { get; set; }

        public class UpdateInfluencerPlatformCommandHandler : CommandHandler<UpdateInfluencerPlatformCommand, InfluencerPlatformViewModel>
        {
            private readonly IGenericRepository<InfluencerPlatform> _influencerPlatformRepository;

            public UpdateInfluencerPlatformCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _influencerPlatformRepository = unitOfWork.Repository<InfluencerPlatform>();
            }

            public override async Task<Response<InfluencerPlatformViewModel>> Handle(UpdateInfluencerPlatformCommand request, CancellationToken cancellationToken)
            {
                var influencerPlatform = await _influencerPlatformRepository.FindSingleAsync(x => x.InfluencerId == request.InfluencerId && x.PlatformId == request.PlatformId);

                Mapper.Map(request, influencerPlatform);

                _influencerPlatformRepository.Update(influencerPlatform);
                await UnitOfWork.CommitAsync();

                var influencerPlatformView = Mapper.Map<InfluencerPlatformViewModel>(influencerPlatform);

                return new Response<InfluencerPlatformViewModel>(influencerPlatformView);
            }
        }
    }
}
