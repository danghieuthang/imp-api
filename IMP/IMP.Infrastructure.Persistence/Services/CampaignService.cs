using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Campaign> _campaignRepostory;
        public CampaignService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _campaignRepostory = _unitOfWork.Repository<Campaign>();
        }

        public async Task AutoUpdateCampaignStatus()
        {
            var campaigns = await _campaignRepostory.GetAll(
                predicate: x => x.Status != (int)CampaignStatus.Draft
                            && x.Status != (int)CampaignStatus.Closed
                            && x.Status != (int)CampaignStatus.Pending
                            && x.Status != (int)CampaignStatus.Cancelled,
                orderBy: x => x.OrderBy(y => y.Created)).ToListAsync();

            foreach (var campaign in campaigns)
            {
                // get Indochina time
                //DateTime now = DateTime.UtcNow.AddHours(7);
                DateTime now = DateTime.UtcNow;
                if (now >= campaign.OpeningDate && now < campaign.ApplyingDate)
                {
                    campaign.Status = (int)CampaignStatus.Openning;
                }
                else if (now >= campaign.ApplyingDate && now < campaign.AdvertisingDate)
                {
                    campaign.Status = (int)CampaignStatus.Applying;
                }
                else if (now >= campaign.AdvertisingDate && now < campaign.EvaluatingDate)
                {
                    campaign.Status = (int)CampaignStatus.Advertising;
                }
                else if (now >= campaign.EvaluatingDate && now < campaign.AnnouncingDate)
                {
                    campaign.Status = (int)CampaignStatus.Evaluating;
                }
                else if (now >= campaign.AnnouncingDate && now < campaign.ClosedDate)
                {
                    await UpdateMemberActivityAfterEvaluating(campaign.Id);
                    campaign.Status = (int)CampaignStatus.Announcing;
                }
                else if (now >= campaign.ClosedDate)
                {
                    campaign.Status = (int)CampaignStatus.Closed;
                    await UpdateRankingOfMemberInCampaign(campaign.Id);
                }
                _campaignRepostory.Update(campaign);

            }
            await _unitOfWork.CommitAsync();

        }

        /// <summary>
        /// Update influencer ranking score of a campaign who completed activity when closed a campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        private async Task UpdateRankingOfMemberInCampaign(int campaignId)
        {
            var campaignMembers = await _unitOfWork.Repository<CampaignMember>().GetAll(
                predicate: x => x.CampaignId == campaignId && x.Status == (int)CampaignMemberStatus.Completed,
                include: x => x.Include(y => y.Influencer).ThenInclude(y => y.Ranking)).ToListAsync();

            if (campaignMembers.Any())
            {
                var rankingRepository = _unitOfWork.Repository<Ranking>();
                var rankLevels = await _unitOfWork.Repository<RankLevel>().GetAll().ToListAsync();
                foreach (var member in campaignMembers)
                {
                    var ranking = member.Influencer.Ranking;
                    ranking.Score += 50; // update score of influencer ranking

                    var rankLevel = rankLevels.Where(x => x.Score <= ranking.Score).OrderByDescending(x => x.Score).FirstOrDefault();
                    if (ranking.RankLevelId != rankLevel.Id)
                    {
                        ranking.RankLevelId = rankLevel.Id;
                    }

                    rankingRepository.Update(ranking);
                }
            }
        }

        public async Task<decimal> BestCampaignMemberTotalProductAmount(int campaignId)
        {
            decimal? value = _unitOfWork.Repository<VoucherCode>().GetAll(
                      predicate: x => x.CampaignMember.CampaignId == campaignId
                  ).Select(x => new
                  {
                      CampaignMemberId = x.CampaignMemberId,
                      TotalProductAmount = x.VoucherTransactions.Sum(y => y.TotalProductAmount)
                  }).OrderByDescending(x => x.TotalProductAmount).Select(x => x.TotalProductAmount).FirstOrDefault();
            return value ?? 0;
        }
        private async Task UpdateMemberActivityAfterEvaluating(int campaignId)
        {
            // get member activities of campaign that has status is not yet
            var memberActivties = await _unitOfWork.Repository<MemberActivity>().GetAll(
                predicate: x => x.CampaignMember.CampaignId == campaignId && x.Status == (int)MemberActivityStatus.NotYet).ToListAsync();

            foreach (var memberActivity in memberActivties)
            {
                memberActivity.Status = (int)MemberActivityStatus.UnComleted; // change campaign member to status unCompleted
                _unitOfWork.Repository<MemberActivity>().Update(memberActivity);
            }

        }
    }
}

