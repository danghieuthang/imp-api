using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public partial class Campaign : BaseEntity
    {
        public Campaign()
        {
            CampaignImages = new List<CampaignImage>();
            Products = new List<Product>();
            Vouchers = new List<CampaignVoucher>();
            CampaignRewards = new List<CampaignReward>();
        }
        [ForeignKey("ApplicationUser")]
        public int CreatedById { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }

        #region Generate Information

        [StringLength(256)]
        public string Title { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [StringLength(2000)]
        public string AdditionalInformation { get; set; }
        [StringLength(2000)]
        public string QA { get; set; }
        [StringLength(256)]
        public string Website { get; set; }
        [StringLength(256)]
        public string Fanpage { get; set; }

        #endregion

        #region Timeline
        /// <summary>
        /// Ngày mở chiến dịch trên web
        /// </summary>
        public DateTime? OpeningDate { get; set; }
        /// <summary>
        /// Ngày nộp đơn & duyệt đơn Advertising: Ngày quảng cáo
        /// </summary>
        public DateTime? ApplyingDate { get; set; }
        /// <summary>
        /// Ngày quảng cáo
        /// </summary>
        public DateTime? AdvertisingDate { get; set; }
        /// <summary>
        /// Ngày đánh giá chiến dịch
        /// </summary>
        public DateTime? EvaluatingDate { get; set; }
        /// <summary>
        /// Ngày thông báo
        /// </summary>
        public DateTime? AnnouncingDate { get; set; }
        /// <summary>
        /// Ngày kết thúc, ngày này bắt buộc phải chuyển hết hoa hồng cho influencer
        /// </summary>
        public DateTime? ClosedDate { get; set; }
        #endregion

        #region Product/service configuration

        [ForeignKey("CampaignType")]
        public int? CampaignTypeId { get; set; }
        [StringLength(2000)]
        public string Keywords { get; set; }
        [StringLength(2000)]
        public string Hashtags { get; set; }
        [StringLength(2000)]
        public string ProductInformation { get; set; }
        [StringLength(2000)]
        public string SampleContent { get; set; }

        #endregion

        #region Reward configuration
        public ICollection<CampaignReward> CampaignRewards { get; set; }
        public int VoucherCommissionMode { get; set; }
        public bool IsPercentVoucherCommission { get; set; }
        public string VoucherCommissionPrices { get; set; }
        #endregion

        [ForeignKey("AppplicationUserId")]
        public int? ApprovedById { get; set; }
        [StringLength(256)]
        public string Note { get; set; }
        public int Status { get; set; }
        public bool IsActived { get; set; }

        public CampaignType CampaignType { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public Brand Brand { get; set; }
        public InfluencerConfiguration InfluencerConfiguration { get; set; }
        public TargetConfiguration TargetConfiguration { get; set; }

        public ICollection<CampaignMember> CampaignMembers { get; set; }
        public ICollection<CampaignVoucher> Vouchers { get; set; }
        public ICollection<CampaignActivity> CampaignActivities { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<CampaignImage> CampaignImages { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
