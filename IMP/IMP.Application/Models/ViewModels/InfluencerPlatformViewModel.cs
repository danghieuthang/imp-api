using IMP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class InfluencerPlatformViewModel : BaseViewModel<int>
    {
        public int InfluencerId { get; set; }
        //public int PlatformId { get; set; }

        //public ApplicationUserViewModel Influencer { get; set; }
        public PlatformViewModel Platform { get; set; }
        public string Url { get; set; }
        public int Follower { get; set; }
        public string Avatar { get; set; }
        public string Hashtag { get; set; }
        public List<string> Interests { get; set; }
    }
}
