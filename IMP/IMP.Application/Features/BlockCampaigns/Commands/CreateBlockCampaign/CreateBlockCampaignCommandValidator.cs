using FluentValidation;
using IMP.Application.Extensions;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.BlockCampaigns.Commands.CreateBlockCampaign
{
    public class CreateBlockCampaignCommandValidator : AbstractValidator<CreateBlockCampaignCommand>
    {
        public CreateBlockCampaignCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.BlockId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<Block>().IsExistAsync(x);
            });

            RuleFor(x => x.CampaignId).MustExistEntityId(async (x, y) =>
            {
                return await unitOfWork.Repository<Campaign>().IsExistAsync(x);
            });
        }
    }
}