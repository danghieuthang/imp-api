using IMP.Application.Enums;
using IMP.Application.Interfaces;
using IMP.Application.Interfaces.Services;
using IMP.Application.Models.ViewModels;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Services
{
    public class MemberActivityService : IMemberActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<MemberActivity> _memberActivityRepository;
        private readonly IFacebookAnalysisService _facebookAnalysisService;

        public MemberActivityService(IUnitOfWork unitOfWork, IFacebookAnalysisService facebookAnalysisService)
        {
            _unitOfWork = unitOfWork;
            _memberActivityRepository = _unitOfWork.Repository<MemberActivity>();
            _facebookAnalysisService = facebookAnalysisService;
        }

        public async Task AutoCheckActivityInSocial()
        {
            var memberActivities = await _memberActivityRepository.GetAll(
                predicate: x => x.Status == (int)MemberActivityStatus.Waiting // Member Activity that waiting for approved
                    && (x.CampaignActivity.Campaign.Status == (int)CampaignStatus.Approved
                        || x.CampaignActivity.Campaign.Status == (int)CampaignStatus.Applying
                        || x.CampaignActivity.Campaign.Status == (int)CampaignStatus.Advertising)
                    && x.CampaignActivity.EvidenceTypeId == 4 // Evidence type is Link post
                    && x.CampaignActivity.Campaign.InfluencerConfiguration.PlatformId == 1, // Platform is facebook
                include: x => x.Include(y => y.Evidences)
                            .Include(y => y.CampaignActivity).ThenInclude(y => y.Campaign)
                     ).ToListAsync();

            foreach (var memberActivity in memberActivities)
            {
                if (memberActivity.Evidences.Count > 0)
                {
                    if (memberActivity.Evidences.FirstOrDefault().Url.Contains("facebook"))
                    {
                        var postSocialContent = await _facebookAnalysisService.GetPost(memberActivity.Evidences.FirstOrDefault().Url);
                        if (postSocialContent != null)
                        {
                            string hashtags = memberActivity.CampaignActivity.Campaign.Hashtags ?? "[]";
                            var activitySocialContent = JsonConvert.DeserializeObject<SocialContent>(memberActivity.SocialContent);

                            var postHashtags = postSocialContent.Hashtags.Select(x => x.Hashtag.Replace("#", "").Replace("\n", "").Replace(" ", "").ToLower());

                            activitySocialContent.Likes = postSocialContent.Likes;
                            activitySocialContent.Shares = postSocialContent.Shares;
                            activitySocialContent.Hashtags.ForEach(x =>
                            {
                                x.IsValid = postHashtags.Contains(x.Hashtag);
                            });

                            memberActivity.SocialContent = JsonConvert.SerializeObject(activitySocialContent);

                            _memberActivityRepository.Update(memberActivity);
                            await _unitOfWork.CommitAsync();
                        }
                        Thread.Sleep(1 * 60 * 1000);
                    }

                }
            }
        }
    }
}
