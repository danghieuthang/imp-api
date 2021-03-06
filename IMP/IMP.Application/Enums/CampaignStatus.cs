using System;
using System.Collections.Generic;
using System.Text;

namespace IMP.Application.Enums
{
    public enum CampaignStatus
    {
        /// <summary>
        /// Nháp
        /// </summary>
        Draft,
        /// <summary>
        /// Đang chờ admin duyệt
        /// </summary>
        Pending,
        /// <summary>
        /// Admin duyệt thành công
        /// </summary>
        Approved,
        /// <summary>
        /// Từ chối duyệt
        /// </summary>
        Cancelled,
        Openning,
        Applying,
        Advertising,
        Evaluating,
        Announcing,
        Closed
    }

    public enum CampaignMemberStatus
    {
        Pending,
        Approved,
        Cancelled,
        Invited,
        RefuseInvitated,
        Completed,
        MakeMoney,
        Closed,
    }

}
