using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
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
    public class GetAllBlockTypeQuery : IGetAllQuery<BlockTypeViewModel>
    {
        public class GetAllBlockTypeQueryHandler : GetAllQueryHandler<GetAllBlockTypeQuery, BlockType, BlockTypeViewModel>
        {
            public GetAllBlockTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
