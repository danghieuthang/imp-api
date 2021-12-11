using AutoMapper;
using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Wrappers;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Application.Features.CampaignMembers.Queries.GetRewards
{
    public class MemberRewardReport
    {
        public List<MemberRewardEarningViewModel> Members { get; set; }
        public decimal Total { get; set; }
    }
    public class MemberRewardEarningViewModel
    {
        public int CampaignMemberId { get; set; }
        public decimal DefaultReward { get; set; }
        public decimal BestInfluencerReward { get; set; }
        public decimal EarningMoney { get; set; }
        public decimal Tax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
    public class GetListRewardQuery : IQuery<MemberRewardReport>
    {
        public int CampaignId { get; set; }
        public class GetListRewardQueryHandler : QueryHandler<GetListRewardQuery, MemberRewardReport>
        {
            private readonly ICampaignService _campaignService;
            public GetListRewardQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICampaignService campaignService) : base(unitOfWork, mapper)
            {
                _campaignService = campaignService;
            }

            public override async Task<Response<MemberRewardReport>> Handle(GetListRewardQuery request, CancellationToken cancellationToken)
            {
                var campaign = await UnitOfWork.Repository<Campaign>().FindSingleAsync(x => x.Id == request.CampaignId, x => x.CampaignRewards, x => x.CampaignMembers);
                if (campaign == null)
                {
                    throw new KeyNotFoundException();
                }

                decimal defaultReward = campaign.CampaignRewards.Where(x => x.IsDefaultReward).Sum(x => x.Price);
                decimal bestInfluencerReward = campaign.CampaignRewards.Where(x => x.IsDefaultReward == false).Sum(x => x.Price);
                var bestInfluencerTotalProductAmount = await _campaignService.BestCampaignMemberTotalProductAmount(request.CampaignId);

                var results = campaign.CampaignMembers.Where(x => x.Status == (int)CampaignMemberStatus.Completed).Select(x => new MemberRewardEarningViewModel
                {
                    DefaultReward = defaultReward,
                    EarningMoney = x.Money,
                    CampaignMemberId = x.Id,
                }).ToList();

                var transactions = await UnitOfWork.Repository<VoucherCode>().GetAll(
                    predicate: x => x.CampaignMember.CampaignId == request.CampaignId)
                    .Select(x => new
                    {
                        x.CampaignMemberId,
                        TotalProductAmount = x.VoucherTransactions.Sum(y => y.TotalProductAmount)
                    }).Where(x => x.TotalProductAmount == bestInfluencerTotalProductAmount).ToListAsync();

                results.ForEach(x =>
                {
                    if (transactions.Any(y => y.CampaignMemberId == x.CampaignMemberId))
                    {
                        x.BestInfluencerReward = bestInfluencerReward;
                    };
                    x.SubTotal = x.DefaultReward + x.BestInfluencerReward + x.EarningMoney;
                    x.Tax = 10 * x.SubTotal / (decimal)100.0;
                    x.Total = x.SubTotal + x.Tax;
                });

                return new Response<MemberRewardReport>(new MemberRewardReport
                {
                    Members = results,
                    Total = results.Sum(x => x.Total)
                } );

            }
        }
    }
}
