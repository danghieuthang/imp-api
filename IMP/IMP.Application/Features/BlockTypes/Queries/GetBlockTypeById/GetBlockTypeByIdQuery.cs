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

        public class GetBlockTypeByIdQueryHandler : GetByIdQueryHandle<GetBlockTypeByIdQuery, BlockType, BlockTypeViewModel>
        {
            public GetBlockTypeByIdQueryHandler(IGenericRepositoryAsync<int, BlockType> repositoryAsync, IMapper mapper) : base(repositoryAsync, mapper)
            {
            }
        }
    }
}
