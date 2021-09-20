using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;

namespace IMP.Application.Features.BlockPlatforms.Commands.CreateBlockPlatform
{
    public class CreateBlockPlatformCommand : ICommand<BlockPlatformViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int BlockId { get; set; }
        public int InfluencerPlatformId { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }

        public class CreateBlockPlatformCommandHander : CommandHandler<CreateBlockPlatformCommand, BlockPlatformViewModel>
        {
            private readonly IGenericRepositoryAsync<BlockPlatform> _blockPlatformRepositoryAsync;
            public CreateBlockPlatformCommandHander(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _blockPlatformRepositoryAsync = unitOfWork.Repository<BlockPlatform>();
            }

            public override async Task<Response<BlockPlatformViewModel>> Handle(CreateBlockPlatformCommand request, CancellationToken cancellationToken)
            {
                var blockPlatform = Mapper.Map<BlockPlatform>(request);

                await _blockPlatformRepositoryAsync.AddAsync(blockPlatform);
                await UnitOfWork.CommitAsync();

                blockPlatform = await _blockPlatformRepositoryAsync.FindSingleAsync(x => x.Id == blockPlatform.Id, includeProperties: x => x.InfluencerPlatform);
                var blockPlatformView = Mapper.Map<BlockCampaignViewModel>(blockPlatform);
                return new Response<BlockPlatformViewModel>();
            }
        }
    }
}