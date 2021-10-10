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

            //RuleFor(x => x.Title).MustMaxLength(256);
            //RuleFor(x => x.Avatar).MustMaxLength(256);
            //RuleFor(x => x.Bio).MustMaxLength(256);
            //RuleFor(x => x.Location).MustMaxLength(256);
            //RuleFor(x => x.Text).MustMaxLength(256);
            //RuleFor(x => x.TextArea).MustMaxLength(2000);
            //RuleFor(x => x.ImageUrl).MustValidUrl(true);
            //RuleFor(x => x.VideoUrl).MustValidUrl(true);

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

            //RuleFor(x => x.ParentId.Value).MustExistEntityId(
            //    async (id, y) => await _blockRepository.IsExistAsync(id)).When(x => x.ParentId.HasValue);
        }

    }
}