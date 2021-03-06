using IMP.Application.Enums;
using IMP.Application.Models.Compaign;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class CampaignVoucherViewModel : BaseViewModel<int>
    {
        public int CampaignId { get; set; }
        public int VoucherId { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int PercentForInfluencer { get; set; }
        public bool IsDefaultReward { get; set; }
        public bool IsBestInfluencerReward { get; set; }
    }

    public class CampaignVoucherOnlyIdViewModel : BaseViewModel<int>
    {
        public int CampaignId { get; set; }
        public CampaignBasicInfoViewModel Campaign { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int PercentForInfluencer { get; set; }
    }
    public class CampaignVoucherRequest
    {
        public int VoucherId { get; set; }
        public int Quantity { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int PercentForInfluencer { get; set; }
    }

    public class VoucherCommissionPrices
    {
        [JsonProperty("from")]
        public decimal? From { get; set; }
        [JsonProperty("to")]
        public decimal? To { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
}
