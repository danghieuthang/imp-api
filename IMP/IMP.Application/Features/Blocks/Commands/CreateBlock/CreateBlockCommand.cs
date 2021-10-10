using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using IMP.Application.Extensions;
using IMP.Application.Helpers;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace IMP.Application.Features.Blocks.Commands.CreateBlock
{
    public class BlockRequest
    {
        public int BlockTypeId { get; set; }
        public string Variant { get; set; }
        public int Position { get; set; }
        public JObject Data { get; set; }
        public List<BlockRequest> ChildBlocks { get; set; }
    }
    public class CreateBlockRequest : BlockRequest
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int PageId { get; set; }
    }

    public class CreateBlockCommand : CreateBlockRequest, ICommand<BlockViewModel>
    {

        public class CreateBlockCommandHandler : CommandHandler<CreateBlockCommand, BlockViewModel>
        {
            private readonly IGenericRepository<Block> _blockRepository;
            public CreateBlockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _blockRepository = unitOfWork.Repository<Block>();
            }

            public override async Task<Response<BlockViewModel>> Handle(CreateBlockCommand request, CancellationToken cancellationToken)
            {
                var block = Mapper.Map<Block>(request);
                block.Enable = true;
                block = await _blockRepository.AddAsync(block);
                await UnitOfWork.CommitAsync();
                var view = Mapper.Map<BlockViewModel>(block);
                return new Response<BlockViewModel>(view);
            }

        }
    }
}