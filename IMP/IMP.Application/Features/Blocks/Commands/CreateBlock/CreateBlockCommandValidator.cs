using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Blocks.Commands.CreateBlock
{
    public class CreateBlockCommandValidator : AbstractValidator<CreateBlockCommand>
    {
        private readonly IGenericRepository<Page> _pageRepository;
        private readonly IGenericRepository<BlockType> _blockTypeRepository;
        private readonly IGenericRepository<Block> _blockRepository;
        public CreateBlockCommandValidator(IUnitOfWork unitOfWork)
        {
            _blockRepository = unitOfWork.Repository<Block>();
            _blockTypeRepository = unitOfWork.Repository<BlockType>();
            _pageRepository = unitOfWork.Repository<Page>();

            RuleFor(x => x.PageId).MustAsync(async (block, id, cancellationToken) =>
            {
                return await _pageRepository.IsExistAsync(x => x.Id == id && x.InfluencerId == block.InfluencerId);
            }).WithMessage("Không hợp lệ.");

            RuleFor(x => x.BlockTypeId).MustExistEntityId(
                async (id, y) => await _blockTypeRepository.IsExistAsync(id));

            RuleForEach(x => x.ChildBlocks).ChildRules((o) =>
            {
                RuleFor(o => o.BlockTypeId).MustExistEntityId(async (id, y) => await _blockTypeRepository.IsExistAsync(id));
            });

        }

    }
}