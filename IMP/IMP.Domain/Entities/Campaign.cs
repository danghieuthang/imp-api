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
        public decimal FixedBonus { get; set; }
        public ICollection<RewardVoucher> RewardVouchers { get; set; }
        [StringLength(50)]
        public string Currency { get; set; }
        public decimal BestInfluencerBonus { get; set; }
        #endregion

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
