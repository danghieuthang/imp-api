using IMP.Application.Models.Compaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class MemberActivityViewModel : BaseViewModel<int>
    {
        public int CampaignMemberId { get; set; }
        public int Status { get; set; }
        public List<ActivityCommentViewModel> ActivityComments { get; set; }
        public List<EvidenceViewModel> Evidences { get; set; }
        public SocialContent SocialContent { get; set; }
        public CampaignMemberBasicInfoViewModel CampaignMember { get; set; }
        public CampaignActivityBasicViewModel CampaignActivity { get; set; }
    }

    public class SocialContent
    {
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Shares { get; set; }
        public int Comments { get; set; }
        public List<HashtagChecker> Hashtags { get; set; }

    }

    public class HashtagChecker
    {
        public string Hashtag { get; set; }
        public bool IsValid { get; set; }
        public bool IsUsedForAuthenticate { get; set; }
    }
}
