using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Enums
{
    public enum NotificationType
    {
        #region administrator
        // brand đăng ký tài khoản xong chờ duyệt
        BrandRegister = 1,
        // Influencer đăng ký tài khoản xong chờ duýet
        InfluencerRegister = 2,
        // Brand vừa tạo chiến dịch(status Pending), chờ admin duyệt
        BrandCreatedCampaign = 3,
        #endregion

        #region Brand
        // Influencer xin thăm gia campaign, chờ brand duỵet
        InfluencerJoinCampaign = 11,
        // Influencer vừa submit xong evidence(trạng thái của member actitivy là Pending), chờ brand duyệt
        InfluencerSubmitMemberActivity = 12,
        AdminApprovedCampaign = 13,
        AdminRejectCampaign = 14,
        AdminPendingCampaign = 15,
        InfluencerCommentMemberActivity = 16,
        // Influencer đồng ý thăm gia chiến dịch\
        InfluencerAcceptedInvited = 17,
        #endregion

        #region Influencer
        // Brand đồng ý cho influencer thăm gia chiến dịch
        BrandApprovedJoinCampaign = 21,
        BrandCancelJoinCampaign = 22,
        // Brand đồng ý với member activity
        BrandApprovedMemberActivity = 23,
        ReceivedMoney = 24,
        BrandAcceptMemberActivity = 25,
        BrandRejectMemberActivity = 26,
        BrandCommentMemberActivity = 27,
        // Brand mời thăm gia campaign
        BrandInvitedToCampaign = 28,
        #endregion
    }
}
