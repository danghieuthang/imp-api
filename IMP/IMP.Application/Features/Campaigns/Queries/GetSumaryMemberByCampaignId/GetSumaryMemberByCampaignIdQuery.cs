using AutoMapper;
using IMP.Application.Enums;
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

namespace IMP.Application.Features.Campaigns.Queries.GetSumaryMemberByCampaignId
{
    public class GetSumaryMemberByCampaignIdQuery : IQuery<SummaryCampaignMemberViewModel>
    {
        public int CampaignId { get; set; }
        public class GetSumaryMemberByCampainIdQueryHandler : QueryHandler<GetSumaryMemberByCampaignIdQuery, SummaryCampaignMemberViewModel>
        {
            public GetSumaryMemberByCampainIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
            {
            }

            public override async Task<Response<SummaryCampaignMemberViewModel>> Handle(GetSumaryMemberByCampaignIdQuery request, CancellationToken cancellationToken)
            {
                var campaignMembers = UnitOfWork.Repository<CampaignMember>().GetAll(
                    predicate: x => x.CampaignId == request.CampaignId,
                    selector: x => new { x.Id, x.Status }).ToList();

                var sumary = new SummaryCampaignMemberViewModel
                {
                    NumberOfApplying = campaignMembers.Count,
                    NumberOfApproved = campaignMembers.Where(x => x.Status == (int)CampaignMemberStatus.Approved).Count(),
                    NumberOfCanceled = campaignMembers.Where(x => x.Status == (int)CampaignMemberStatus.Cancelled).Count(),
                    NumberOfPending = campaignMembers.Where(x => x.Status == (int)CampaignMemberStatus.Pending).Count(),
                };
                return new Response<SummaryCampaignMemberViewModel>(sumary);
            }
        }
    }
}
