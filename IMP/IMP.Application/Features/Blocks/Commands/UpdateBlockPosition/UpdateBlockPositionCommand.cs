using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.Blocks.Commands.UpdateBlockPosition
{
    public class UpdateBlockPositionCommand : ICommand<int>
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public int FromPosition { get; set; }
        public int ToPosition { get; set; }
        [JsonIgnore]
        public int InfluencerId { get; set; }
    }
    public class UpdateBlockPositionCommandHandler : CommandHandler<UpdateBlockPositionCommand, int>
    {
        private readonly IGenericRepository<Block> _blockRepository;
        public UpdateBlockPositionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _blockRepository = unitOfWork.Repository<Block>();
        }

        public override async Task<Response<int>> Handle(UpdateBlockPositionCommand request, CancellationToken cancellationToken)
        {
            var fromBlock = await _blockRepository.FindSingleAsync(x => x.Id == request.FromId);
            var toblock = await _blockRepository.FindSingleAsync(x => x.Id == request.ToId);

            ValidationBeforeUpdate(fromBlock, toblock, request.InfluencerId);

            fromBlock.Position = request.FromPosition;
            toblock.Position = request.ToPosition;

            _blockRepository.Update(fromBlock);
            _blockRepository.Update(toblock);
            await UnitOfWork.CommitAsync();

            return new Response<int>(toblock.Id);

        }
        private void ValidationBeforeUpdate(Block blockFrom, Block blockTo, int influencerId)
        {
            List<ValidationError> errors = new();
            if (blockFrom == null)
            {
                errors.Add(new ValidationError("from_id", "Không tồn tại."));
            }
            if (blockTo == null)
            {
                errors.Add(new ValidationError("from_id", "Không tồn tại."));
            }
            //check permision
            //if ((blockFrom != null && blockFrom.Page?.InfluencerId != influencerId) || (blockTo != null && blockTo.Page?.InfluencerId != influencerId))
            //{
            //    errors.Add(new ValidationError("", "Không có quyền chỉnh sửa."));

            //};
            if (errors.Count > 0)
                throw new ValidationException(errors);
        }
    }
}
