﻿using System;
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
        public const string Wallet = Common + "wallets";
        public const string WalletTransaction = Common + "wallet-transactions";
        public const string Milestones = Common + "milestones";
    }
}
