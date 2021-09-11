using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Features.BlockTypes.Commands.DeleteBlockType
{
    public class DeleteBlockTypeCommand : IDeleteCommand<BlockType>
    {
        public int Id { get; set; }

        public class DeleteBockTypeCommandHandler : DeleteCommandHandler<BlockType, DeleteBlockTypeCommand>
        {
            public DeleteBockTypeCommandHandler(IGenericRepositoryAsync<int, BlockType> repositoryAsync) : base(repositoryAsync)
            {
            }
        }
    }
}
