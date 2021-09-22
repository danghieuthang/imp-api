using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;

namespace IMP.Application.Features.BlockPlatforms.Commands.UpdateBlockPlatform
{
    public class UpdateBlockPlatformCommand : ICommand<BlockPlatformViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int Id { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }

        public class UpdateBlockPlatformCommandHandler : CommandHandler<UpdateBlockPlatformCommand, BlockPlatformViewModel>
        {
            private readonly IGenericRepository<BlockPlatform> _blockPlatformRepositoryAsync;
            public UpdateBlockPlatformCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _blockPlatformRepositoryAsync = unitOfWork.Repository<BlockPlatform>();
            }

            public override async Task<Response<BlockPlatformViewModel>> Handle(UpdateBlockPlatformCommand request, CancellationToken cancellationToken)
            {
                var blockPlatform = await _blockPlatformRepositoryAsync.FindSingleAsync(x => x.Id == request.Id, includeProperties: x => x.InfluencerPlatform);
                if (blockPlatform != null)
                {
                    blockPlatform.Position = request.Position;
                    blockPlatform.IsActived = request.IsActived;
                    _blockPlatformRepositoryAsync.Update(blockPlatform);
                    await UnitOfWork.CommitAsync();
                    var blockPlatformView = Mapper.Map<BlockPlatformViewModel>(blockPlatform);
                    return new Response<BlockPlatformViewModel>(data: blockPlatformView);
                }
                throw new ValidationException(new ValidationError("id", $"'{request.Id}' không tồn tại."));
            }
        }
    }
}