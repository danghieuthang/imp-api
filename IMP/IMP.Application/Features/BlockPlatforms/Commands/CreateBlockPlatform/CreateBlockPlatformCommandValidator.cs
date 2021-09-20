using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.BlockPlatforms.Commands.CreateBlockPlatform
{
    public class CreateBlockPlatformCommandValidator : AbstractValidator<CreateBlockPlatformCommand>
    {
        public CreateBlockPlatformCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.BlockId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<BlockPlatform>().IsExistAsync(x);
            }).DependentRules(
                () =>
                {
                    RuleFor(x => x.InfluencerId).MustAsync(async (x, y, z) =>
                    {
                        var block = await unitOfWork.Repository<Block>().FindSingleAsync(block => block.Id == x.BlockId, includeProperties: block => block.Page);
                        if (block != null && block.Page.InfluencerId == x.InfluencerId)
                        {
                            return true;
                        }
                        return false;
                    }).WithMessage("Không có quyền tạo.");
                }
            );

            RuleFor(x => x.InfluencerPlatformId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<InfluencerPlatform>().IsExistAsync(x);
            });
        }
    }
}