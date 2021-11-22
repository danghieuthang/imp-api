using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi
{
    public class RouterConstants
    {
        public const string Common = "api/v{version:apiVersion}/";
        public const string Account = "api/accounts";
        public const string Platform = Common + "platforms";
        public const string CampaignType = Common + "campaign-types";
        public const string Campaign = Common + "campaigns";
        public const string BlockType = Common + "block-types";
        public const string InfluencerPlatform = Common + "influencer-platforms";
        public const string Location = Common + "locations";
        public const string User = Common + "users";
        public const string Bank = Common + "banks";
        public const string File = Common + "files";
        public const string Page = Common + "pages";
        public const string Block = Common + "blocks";
        public const string BlockPlatform = Common + "block-platforms";
        public const string BlockCampaign = Common + "block-campaigns";
        public const string Voucher = Common + "vouchers";
        public const string VoucherCode = Common + "voucher-codes";
        public const string Wallet = Common + "wallets";
        public const string WalletTransaction = Common + "wallet-transactions";
        public const string Milestones = Common + "milestones";
        public const string Brand = Common + "brands";
        public const string RankLevel = Common + "rank-level";
        public const string ActivityType = Common + "activity-types";
        public const string Group = Common + "groups";
        public const string Interest = Common + "interests";
        public const string Job = Common + "jobs";
        public const string CampaignVoucher = Common + "campaign-vouchers";
        public const string Influencer = Common + "influencers";
        public const string CampaignMember = Common + "campaign-members";
        public const string MemberActivity = Common + "member-activities";
        public const string Evidence = Common + "evidences";
        public const string EvidenceType = Common + "evidence-types";
        public const string ActivityComment = Common + "activity-comments";
        public const string Admin = Common + "admin";
    }
}
