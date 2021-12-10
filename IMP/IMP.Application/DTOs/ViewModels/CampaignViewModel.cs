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
        public int VoucherCommissionMode { get; set; }
        public bool IsPercentVoucherCommission { get; set; }
        #endregion
        public int? ApprovedById { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public bool IsActived { get; set; }

        public BrandViewModel Brand { get; set; }
        public InfluencerConfigurationViewModel InfluencerConfiguration { get; set; }
        public TargetConfigurationViewModel TargetConfiguration { get; set; }
        public List<VoucherCommissionPrices> VoucherCommissionPrices { get; set; }
        public List<CampaignRewardViewModel> DefaultRewards { get; set; }
        public List<CampaignRewardViewModel> BestInfluencerRewards { get; set; }
        public List<CampaignActivityViewModel> CampaignActivities { get; set; }
        public List<CampaignVoucherViewModel> Vouchers { get; set; }
        public List<CampaignVoucherViewModel> DefaultVoucherRewards { get; set; }
        public List<CampaignVoucherViewModel> BestInfluencerVoucherRewards { get; set; }
        [JsonProperty("images")]
        public List<CampaignImageViewModel> CampaignImages { get; set; }
    }
    public class CampaignNotificationViewModel : BaseViewModel<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
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
    public class CampaignBasicInfoViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [JsonProperty("images")]
        public List<CampaignImageViewModel> CampaignImages { get; set; }
    }

    public class CampaignReportViewModel
    {
        public int NumberOfInfluencer { get; set; }
        public int NumberOfInfluencerApply { get; set; }
        public int NumberOfInfluencerCompleted { get; set; }
        public int NumberOfVoucher { get; set; }
        public int NumberOfVoucherCode { get; set; }
        public int TotalNumberVoucherCodeUsed { get; set; }
        public int TotalNumberVoucherCodeGet { get; set; }
        public int TotalNumberVoucherCodeQuantity { get; set; }
        public decimal TotalProductAmount { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public int TotalTransaction { get; set; }
        public decimal TotalEarningMoney { get; set; }
        public List<CampaignMemberReportViewModel> CampaignMembers { get; set; }
    }

    public class CampaignMemberReportViewModel
    {
        public UserBasicViewModel Influencer { get; set; }
        public int QuantityVoucherGet { get; set; }
        public int QuantityVoucherUsed { get; set; }

        public int Status { get; set; }
        public bool IsBestInfluencer { get; set; }

        public int TotalTransaction { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalProductAmount { get; set; }
        public int TotalVoucherCode { get; set; }
        public decimal TotalEarningAmount { get; set; }

    }
}
