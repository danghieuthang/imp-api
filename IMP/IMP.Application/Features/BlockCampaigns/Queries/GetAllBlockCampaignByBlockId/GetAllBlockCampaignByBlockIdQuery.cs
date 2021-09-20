using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;

namespace IMP.Application.Features.BlockCampaigns.Queries.GetAllBlockCampaignByBlockId
{
    public class GetAllBlockCampaignByBlockIdQuery : IGetAllQuery<BlockCampaignViewModel>
    {
        public int BlockId { get; set; }
        public class GetAllBlockPlatformByBlockIdQueryHandler : GetAllQueryHandler<GetAllBlockCampaignByBlockIdQuery, BlockCampaign, BlockCampaignViewModel>
        {
            public GetAllBlockPlatformByBlockIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<BlockCampaignViewModel>>> Handle(GetAllBlockCampaignByBlockIdQuery request, CancellationToken cancellationToken)
            {
                var blockCampaigns = await Repository.FindAllAsync(x => x.BlockId == request.BlockId, includeProperties: x => x.Campaign);
                var blockCampaignViews = Mapper.Map<IEnumerable<BlockCampaignViewModel>>(blockCampaigns);
                return new Response<IEnumerable<BlockCampaignViewModel>>(blockCampaignViews);
            }
        }
    }
}