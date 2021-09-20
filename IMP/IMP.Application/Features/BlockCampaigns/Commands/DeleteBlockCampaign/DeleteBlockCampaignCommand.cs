using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;

namespace IMP.Application.Features.BlockCampaigns.Commands.DeleteBlockCampaign
{
    public class DeleteBlockCampaignCommand : IDeleteCommand<BlockCampaign>
    {
        public int Id { get; set; }
        public int InfluencerId { get; init; }
        public class DeleteBlockCampaignCommandValidator : AbstractValidator<DeleteBlockCampaignCommand>
        {
            public DeleteBlockCampaignCommandValidator(IUnitOfWork unitOfWork)
            {
                RuleFor(x => x.Id).MustAsync(async (blockCampaign, id, cancellationToken) =>
                {
                    var entity = await unitOfWork.Repository<BlockCampaign>().FindSingleAsync(x => x.Id == id, x => x.Block, x => x.Block.Page);
                    if (entity != null && entity.Block.Page.InfluencerId == blockCampaign.InfluencerId)
                    {
                        return true;
                    }
                    return false;
                });
            }
        }
        public class DeleteBlockCampaignCommandHandler : DeleteCommandHandler<BlockCampaign, DeleteBlockCampaignCommand>
        {
            public DeleteBlockCampaignCommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }
        }
    }
}