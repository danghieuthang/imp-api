using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class MemberActivityViewModel : BaseViewModel<int>
    {
        public int CampaignActivityId { get; set; }
        public int CampaignMemberId { get; set; }
        public bool Status { get; set; }
        public List<ActivityCommentViewModel> ActivityComments { get; set; }
        public List<EvidenceViewModel> Evidences { get; set; }
    }
}
