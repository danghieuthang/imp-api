using AutoMapper;
using IMP.Application.Interfaces;
using IMP.Application.Models.ViewModels;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignMembers.Queries.GetCampaignMemberById
{
    public class GetCampaignMemberByIdQuery : IGetByIdQuery<CampaignMember, CampaignMemberViewModel>
    {
        public int Id { get; set; }
        public class GetCampaignMemberByIdQueryHandler : GetByIdQueryHandler<GetCampaignMemberByIdQuery, CampaignMember, CampaignMemberViewModel>
        {
            public GetCampaignMemberByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<CampaignMemberViewModel>> Handle(GetCampaignMemberByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await Repository.FindSingleAsync(x => x.Id == request.Id, x => x.Influencer, x => x.ApprovedBy, x => x.MemberActivities);
                if (entity == null)
                {
                    throw new KeyNotFoundException();
                }

                var data = Mapper.Map<CampaignMemberViewModel>(entity);
                return new Response<CampaignMemberViewModel>(data);
            }
        }
    }
}
