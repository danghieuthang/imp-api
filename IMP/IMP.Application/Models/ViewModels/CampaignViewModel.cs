using IMP.Application.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.Compaign
{
    public class CampaignViewModel : BaseViewModel<int>
    {
        public int CreatedById { get; set; }
        #region Generate Information

        public string Title { get; set; }
        public string Description { get; set; }
        public string AdditionalInformation { get; set; }
        public string QA { get; set; }
        public List<string> Websites { get; set; }
        public List<string> Fanpages { get; set; }

        #endregion

        #region Time line
        /// <summary>
        /// Ngày mở chiến dịch trên web
        /// </summary>
        public DateTime? OpeningDate { get; set; }
        /// <summary>
        /// Ngày nộp đơn & duyệt đơn Advertising: Ngày quảng cáo
        /// </summary>
        [JsonProperty("applying_date")]
        public DateTime? ApplyingDate { get; set; }
        /// <summary>
        /// Ngày quảng cáo
        /// </summary>
        [JsonProperty("advertising_date")]
        public DateTime? AdvertisingDate { get; set; }
        /// <summary>
        /// Ngày đánh giá chiến dịch
        /// </summary>
        [JsonProperty("evaluating_date")]
        public DateTime? EvaluatingDate { get; set; }
        /// <summary>
        /// Ngày thông báo
        /// </summary>
        [JsonProperty("announcing_date")]
        public DateTime? AnnouncingDate { get; set; }
        /// <summary>
        /// Ngày kết thúc, ngày này bắt buộc phải chuyển hết hoa hồng cho influencer
        /// </summary>
        [JsonProperty("closed_date")]
        public DateTime? ClosedDate { get; set; }
        #endregion

        #region Product/service configuration

        public CampaignTypeViewModel CampaignType { get; set; }
        public List<ProductViewModel> Products { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Hashtags { get; set; }
        public string ProductInformation { get; set; }
        public string SampleContent { get; set; }

        #endregion

        #region Reward configuration

        #endregion
        public int? ApprovedById { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public bool IsActived { get; set; }

        public BrandViewModel Brand { get; set; }
        public InfluencerConfigurationViewModel InfluencerConfiguration { get; set; }
        public TargetConfigurationViewModel TargetConfiguration { get; set; }
        public List<CampaignRewardViewModel> DefaultRewards { get; set; }
        public List<CampaignRewardViewModel> BestInfluencerRewards { get; set; }
        public List<CampaignActivityViewModel> CampaignActivities { get; set; }
        public List<CampaignVoucherViewModel> Vouchers { get; set; }
        public List<CampaignVoucherViewModel> DefaultVoucherRewards { get; set; }
        public List<CampaignVoucherViewModel> BestInfluencerVoucherRewards { get; set; }
        [JsonProperty("images")]
        public List<CampaignImageViewModel> CampaignImages { get; set; }
    }

    public class CampaignImageViewModel : BaseViewModel<int>
    {
        public int Position { get; set; }
        public string Url { get; set; }
    }

    public class CampaignImageRequest
    {
        public int Position { get; set; }
        public string Url { get; set; }
    }

    public class CampaignRewardRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
