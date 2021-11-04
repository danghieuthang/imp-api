using IMP.Application.Models.ViewModels;
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

        public int BrandId { get; set; }

        #region Generate Information

        public string Title { get; set; }
        public string Description { get; set; }
        public string AdditionalInformation { get; set; }
        public string QA { get; set; }
        public List<string> Websites { get; set; }
        public List<string> Fanpages { get; set; }

        #endregion

        #region Timeline
        /// <summary>
        /// Ngày mở chiến dịch trên web
        /// </summary>
        public DateTime? Openning { get; set; }
        /// <summary>
        /// Ngày nộp đơn & duyệt đơn Advertising: Ngày quảng cáo
        /// </summary>
        public DateTime? Applying { get; set; }
        /// <summary>
        /// Ngày quảng cáo
        /// </summary>
        public DateTime? Advertising { get; set; }
        /// <summary>
        /// Ngày đánh giá chiến dịch
        /// </summary>
        public DateTime? Evaluating { get; set; }
        /// <summary>
        /// Ngày thông báo
        /// </summary>
        public DateTime? Announcing { get; set; }
        /// <summary>
        /// Ngày kết thúc, ngày này bắt buộc phải chuyển hết hoa hồng cho influencer
        /// </summary>
        public DateTime? Closed { get; set; }
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

        public int Status { get; set; }
        public bool IsActived { get; set; }

        public BrandViewModel Brand { get; set; }
        public InfluencerConfigurationViewModel InfluencerConfiguration { get; set; }
        public TargetConfigurationViewModel TargetConfiguration { get; set; }
        public List<CampaignRewardViewModel> DefaultRewards { get; set; }
        public List<CampaignRewardViewModel> BestInfluencerRewards { get; set; }
        public List<CampaignActivityViewModel> CampaignActivities { get; set; }
        public List<CampaignVoucherViewModel> Vouchers { get; set; }
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
