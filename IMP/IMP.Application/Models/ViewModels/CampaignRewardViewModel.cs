using Newtonsoft.Json;
using System.Collections.Generic;

namespace IMP.Application.Models.Compaign
{
    public class CampaignRewardViewModel : BaseViewModel<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int RewardType { get; set; }
        public List<IntervalRewardViewModel> IntervalRewards { get; set; }
    }

    public class IntervalRewardViewModel
    {
        [JsonProperty("interval")]
        public string Interval { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}