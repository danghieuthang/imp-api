﻿using System;
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
    }
}
