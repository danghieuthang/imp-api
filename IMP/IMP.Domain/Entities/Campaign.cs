using IMP.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IMP.Domain.Entities
{
    public class Campaign : BaseEntity
    {
        [ForeignKey("Platform")]
        public int PlatformId { get; set; }

        [ForeignKey("CampaignType")]
        public int CampaignTypeId { get; set; }

        [ForeignKey("ApplicationUser")]
        public int CreatedById { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }


        [StringLength(256)]
        public string Title { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [StringLength(2000)]
        public string AdditionalInformation { get; set; }
        [StringLength(2000)]
        public string ProductInformation { get; set; }
        [StringLength(2000)]
        public string Reward { get; set; }
        [StringLength(256)]
        public string ReferralWebsite { get; set; }
        public string Keywords { get; set; }
        public int MaxInfluencer { get; set; }
        public decimal PrizeMoney { get; set; }
        public bool IsPrizePerVoucher { get; set; }
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
        public DateTime? Closeing { get; set; }
        #endregion
        public int Status { get; set; }
        public bool IsActived { get; set; }

        public Platform Platform { get; set; }
        public CampaignType CampaignType { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public Brand Brand { get; set; }
        public CampaignCondition CampaignCondition { get; set; }

        public ICollection<CampaignMember> CampaignMembers { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }
        public ICollection<CampaignActivity> CampaignActivities { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<CampaignImage> CampaignImages { get; set; }
    }
}
