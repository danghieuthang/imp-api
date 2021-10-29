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
        public string Website { get; set; }
        public string Fanpage { get; set; }

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

        public int CampaignTypeId { get; set; }
        public string Keywords { get; set; }
        public string Hashtags { get; set; }
        public string ProductInformation { get; set; }
        public string SampleContent { get; set; }

        #endregion

        #region Reward configuration
        public decimal FixedBonus { get; set; }
        public string Currency { get; set; }
        public decimal BestInfluencerBonus { get; set; }
        #endregion

        public int Status { get; set; }
        public bool IsActived { get; set; }

        public BrandViewModel Brand { get; set; }
        public InfluencerConfigurationViewModel InfluencerConfiguration { get; set; }
        public TargetConfigurationViewModel TargetConfiguration { get; set; }
    }

    public class CampaignImageViewModel : BaseViewModel<int>
    {
        public int Position { get; set; }
        public string Url { get; set; }
    }
}
