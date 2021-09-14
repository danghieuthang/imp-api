using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.BlockTypes.Queries.GetBlockTypeById
{
    public class GetBlockTypeByIdQuery : IGetByIdQuery<BlockType, BlockTypeViewModel>
    {
        public int Id { get; set; }

        public class GetBlockTypeByIdQueryHandler : GetByIdQueryHandler<GetBlockTypeByIdQuery, BlockType, BlockTypeViewModel>
        {
            public GetBlockTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }
        }
    }
}
