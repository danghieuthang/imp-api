using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.BlockTypes.Queries.GetAllBlockType
{
    public class GetAllBlockTypeQuery : IRequest<Response<IEnumerable<BlockType>>>
    {
        public class GetAllBlockTypeQueryHandler : IRequestHandler<GetAllBlockTypeQuery, Response<IEnumerable<BlockType>>>
        {
            private readonly IGenericRepository<int, BlockType> _blockTypeResponsitoryAsync;
            private readonly IMapper _mapper;

            public GetAllBlockTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _blockTypeResponsitoryAsync = unitOfWork.Repository<BlockType>();
                _mapper = mapper;
            }

            public async Task<Response<IEnumerable<BlockType>>> Handle(GetAllBlockTypeQuery request, CancellationToken cancellationToken)
            {
                var blockTypes = await _blockTypeResponsitoryAsync.GetAllAsync();
                var blockTypeViews = _mapper.Map<IEnumerable<BlockType>>(blockTypes);
                return new Response<IEnumerable<BlockType>>(blockTypeViews);
            }
        }
    }
}
