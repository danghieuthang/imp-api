using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;

namespace IMP.Application.Features.BlockPlatforms.Queries.GetAllBlockPlatformByBlockId
{
    public class GetAllBlockPlatformByBlockIdQuery : IGetAllQuery<BlockPlatformViewModel>
    {
        public int BlockId { get; set; }
        public class GetAllBlockPlatformByBlockIdQueryHandler : GetAllQueryHandler<GetAllBlockPlatformByBlockIdQuery, BlockPlatform, BlockPlatformViewModel>
        {
            public GetAllBlockPlatformByBlockIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<BlockPlatformViewModel>>> Handle(GetAllBlockPlatformByBlockIdQuery request, CancellationToken cancellationToken)
            {
                var blockPlatforms = await Repository.FindAllAsync(x => x.BlockId == request.BlockId, includeProperties: x => x.InfluencerPlatform);
                var blockPlatformViews = Mapper.Map<IEnumerable<BlockPlatformViewModel>>(blockPlatforms);
                return new Response<IEnumerable<BlockPlatformViewModel>>(blockPlatformViews);
            }

        }
    }
}