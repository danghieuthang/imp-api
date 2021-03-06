using AutoMapper;
using IMP.Application.Models.Compaign;
using IMP.Application.Interfaces.Repositories;
using IMP.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IMP.Application.Interfaces;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using IMP.Application.Enums;

namespace IMP.Application.Features.Campaigns.Queries.GetCampaignById
{
    public class GetCampaignByIdQuery : IGetByIdQuery<Campaign, CampaignViewModel>
    {
        public int Id { get; set; }
        public class GetCampaignByIdQueryHandler : GetByIdQueryHandler<GetCampaignByIdQuery, Campaign, CampaignViewModel>
        {
            public GetCampaignByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CampaignViewModel>> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await Repository.FindSingleAsync(x => x.Id == request.Id,
                    include: c => c.Include(x => x.CampaignImages)
                            .Include(x => x.Products)
                            .Include(x => x.CampaignRewards)
                            .Include(x => x.Vouchers)
                            .Include(x => x.TargetConfiguration).ThenInclude(x => x.Locations).ThenInclude(y => y.Location)
                            .Include(x => x.InfluencerConfiguration).ThenInclude(x => x.Locations).ThenInclude(y => y.Location)
                            .Include(x => x.InfluencerConfiguration).ThenInclude(x => x.Platform)
                            .Include(x => x.CampaignActivities)
                            .Include(x => x.CampaignType)
                            .Include(x => x.Brand));

                if (entity == null)
                {
                    //var error = new ValidationError("id", $"'{request.Id}' không tồn tại");
                    throw new KeyNotFoundException();
                }

                int countMemberApplyToCampaign = await UnitOfWork.Repository<CampaignMember>().CountAsync(x =>
                       x.CampaignId == request.Id
                       && x.Status == (int)CampaignMemberStatus.Approved);

                var data = Mapper.Map<CampaignViewModel>(entity);
                if (data.InfluencerConfiguration != null)
                    data.InfluencerConfiguration.NumberOfJoinedInfluencer = countMemberApplyToCampaign;
                return new Response<CampaignViewModel>(data);
            }
        }
    }


}
