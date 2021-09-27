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
        //public bool IsActived { get; set; }

        public class CreateBlockPlatformCommandHander : CommandHandler<CreateBlockPlatformCommand, BlockPlatformViewModel>
        {
            private readonly IGenericRepository<BlockPlatform> _blockPlatformRepository;
            public CreateBlockPlatformCommandHander(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _blockPlatformRepository = unitOfWork.Repository<BlockPlatform>();
            }

            public override async Task<Response<BlockPlatformViewModel>> Handle(CreateBlockPlatformCommand request, CancellationToken cancellationToken)
            {
                var blockPlatform = Mapper.Map<BlockPlatform>(request);
                blockPlatform.IsActived = true;
                await _blockPlatformRepository.AddAsync(blockPlatform);
                await UnitOfWork.CommitAsync();

                blockPlatform = await _blockPlatformRepository.FindSingleAsync(x => x.Id == blockPlatform.Id, includeProperties: x => x.InfluencerPlatform);
                var blockPlatformView = Mapper.Map<BlockPlatformViewModel>(blockPlatform);
                return new Response<BlockPlatformViewModel>(blockPlatformView);
            }
        }
    }
}