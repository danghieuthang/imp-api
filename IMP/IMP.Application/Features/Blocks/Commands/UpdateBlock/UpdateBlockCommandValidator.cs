using System.Data;
using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.Blocks.Commands.UpdateBlock
{
    public class UpdateBlockCommandValidator : AbstractValidator<UpdateBlockCommand>
    {
        private readonly IGenericRepository<Page> _pageRepositoryAsync;
        private readonly IGenericRepository<BlockType> _blockTypeRepositoryAsync;
        private readonly IGenericRepository<Block> _blockRepositoryAsync;
        public UpdateBlockCommandValidator(IUnitOfWork unitOfWork)
        {
            _blockRepositoryAsync = unitOfWork.Repository<Block>();
            _blockTypeRepositoryAsync = unitOfWork.Repository<BlockType>();
            _pageRepositoryAsync = unitOfWork.Repository<Page>();

            RuleFor(x => x.Title).MustMaxLength(256);
            RuleFor(x => x.Avatar).MustMaxLength(256);
            RuleFor(x => x.Bio).MustMaxLength(256);
            RuleFor(x => x.Location).MustMaxLength(256);
            RuleFor(x => x.Text).MustMaxLength(256);
            RuleFor(x => x.TextArea).MustMaxLength(2000);
            RuleFor(x => x.ImageUrl).MustValidUrl();
            RuleFor(x => x.VideoUrl).MustValidUrl();

            RuleFor(x => x.Id).MustAsync(async (block, id, cancellationToken) =>
            {
                var b = await _blockRepositoryAsync.FindSingleAsync(x => x.Id == id, x => x.Page);
                if (b == null)
                {
                    return false;
                }
                // if this block is create by influencer
                return b.Page.InfluencerId == block.InfluencerId;
            }).WithMessage("'{PropertyValue}' không hợp lệ");

            RuleFor(x => x.PageId).MustAsync(async (block, id, cancellationToken) =>
            {
                return await _pageRepositoryAsync.IsExistAsync(x => x.Id == id && x.InfluencerId == block.InfluencerId);
            }).WithMessage("Không hợp lệ.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Position).MustAsync(
                async (block, position, cancellationToken) =>
                {
                    return await _blockRepositoryAsync.IsExistAsync(x => x.PageId == block.PageId && x.Position == position);
                }
            ).WithMessage("Đã tồn tại.");
            });

            RuleFor(x => x.BlockTypeId).MustExistEntityId(
                async (id, y) => await _blockTypeRepositoryAsync.IsExistAsync(id));

            RuleFor(x => x.ParentId).MustExistEntityId(
                async (id, y) => await _blockRepositoryAsync.IsExistAsync(id));
        }
    }
}