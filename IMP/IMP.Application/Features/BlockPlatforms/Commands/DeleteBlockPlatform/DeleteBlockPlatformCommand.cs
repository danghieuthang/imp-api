using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.BlockPlatforms.Commands.DeleteBlockPlatform
{
    public class DeleteBlockPlatformCommand : IDeleteCommand<BlockPlatform>
    {
        public int InfluencerId { get; set; }
        public int Id { get; set; }
        public class DeleteBlockPlatformCommandHandler : DeleteCommandHandler<BlockPlatform, DeleteBlockPlatformCommand>
        {
            public DeleteBlockPlatformCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
        }
    }
}