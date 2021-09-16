using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMP.WebApi
{
    public class RouterConstants
    {
        public const string COMMOM = "api/v{version:apiVersion}/";
        public const string ACCOUNT = "api/accounts";
        public const string PLATFORM = COMMOM + "platforms";
        public const string CAMPAIGN_TYPE = COMMOM + "campaign-types";
        public const string CAMPAIGN = COMMOM + "campaigns";
        public const string BLOCK_TYPE = COMMOM + "block-types";
        public const string INFLUENCER_PLATFORM = COMMOM + "influencer-platforms";
        public const string LOCATION = COMMOM + "locations";
        public const string USER = COMMOM + "users";
        public const string BANK = COMMOM + "banks";
        public const string FILE = COMMOM + "files";
        public const string PAGE = COMMOM + "pages";
    }
}
