using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Blocks.Queries
{
    public class GetAllBlockByPageIdQuery : IGetAllQuery<BlockViewModel>
    {
        public int PageId { get; set; }
        public class GetAllBlockOfPageQueryHanlder : GetAllQueryHandler<GetAllBlockByPageIdQuery, Block, BlockViewModel>
        {
            public GetAllBlockOfPageQueryHanlder(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<IEnumerable<BlockViewModel>>> Handle(GetAllBlockByPageIdQuery request, CancellationToken cancellationToken)
            {
                var blocks = await Repository.FindAllAsync(x => x.PageId == request.PageId);
                var blockViews = Mapper.Map<IEnumerable<BlockViewModel>>(blocks);
                return new Response<IEnumerable<BlockViewModel>>(blockViews);
            }
        }
    }
}