using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using IMP.Application.Exceptions;
using IMP.Application.Interfaces;
using IMP.Application.Models;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Newtonsoft.Json;

namespace IMP.Application.Features.BlockCampaigns.Commands.UpdateBlockCampaign
{
    public class UpdateBlockCampaignCommand : ICommand<BlockCampaignViewModel>
    {
        [JsonIgnore]
        public int InfluencerId { get; set; }
        public int Id { get; set; }
        public int Position { get; set; }
        public bool IsActived { get; set; }
        public class UpdateBlockCampaignCommandHandler : CommandHandler<UpdateBlockCampaignCommand, BlockCampaignViewModel>
        {
            private readonly IGenericRepositoryAsync<BlockCampaign> _blockCampaignRepositoryAsync;
            public UpdateBlockCampaignCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
                _blockCampaignRepositoryAsync = unitOfWork.Repository<BlockCampaign>();
            }

            public override async Task<Response<BlockCampaignViewModel>> Handle(UpdateBlockCampaignCommand request, CancellationToken cancellationToken)
            {
                var blockCampaign = await _blockCampaignRepositoryAsync.FindSingleAsync(x => x.Id == request.Id, includeProperties: x => x.Campaign);
                if (blockCampaign != null)
                {
                    blockCampaign.Position = request.Position;
                    blockCampaign.IsActived = request.IsActived;

                    _blockCampaignRepositoryAsync.Update(blockCampaign);
                    await UnitOfWork.CommitAsync();

                    var blockCampaignView = Mapper.Map<BlockCampaignViewModel>(blockCampaign);
                    return new Response<BlockCampaignViewModel>(blockCampaignView);
                }
                throw new ValidationException(new ValidationError("id", $"'{request.Id}' không tồn tại."));

            }
        }

    }
}