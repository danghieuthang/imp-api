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
        public const string CAMPAIGN_TYPE  = COMMOM +"campaign-types";
        public const string CAMPAIGN  = COMMOM +"campaigns";
        public const string BLOCK_TYPE  = COMMOM +"block-types";
    }
}
