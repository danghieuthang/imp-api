using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.Compaign;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;

namespace IMP.Application.Features.BlockCampaigns.Commands.CreateBlockCampaign
{
    public class CreateBlockCampaignCommand : ICommand<BlockCampaignViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int CampaignId { get; set; }
        public int BlockId { get; set; }
        public int Position { get; set; }
        //public bool IsActived { get; set; }
        public class CreateBlockCampaignCommandHandler : CommandHandler<CreateBlockCampaignCommand, BlockCampaignViewModel>
        {
            public CreateBlockCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<BlockCampaignViewModel>> Handle(CreateBlockCampaignCommand request, CancellationToken cancellationToken)
            {
                var repository = UnitOfWork.Repository<BlockCampaign>();
                var blockCampaign = Mapper.Map<BlockCampaign>(request);
                blockCampaign.IsActived = true;

                await repository.AddAsync(blockCampaign);
                await UnitOfWork.CommitAsync();

                blockCampaign = await repository.FindSingleAsync(x => x.Id == blockCampaign.Id, includeProperties: x => x.Campaign);
                var blockCampaignView = Mapper.Map<BlockCampaignViewModel>(blockCampaign);
                return new Response<BlockCampaignViewModel>(blockCampaignView);
            }
        }
    }
}