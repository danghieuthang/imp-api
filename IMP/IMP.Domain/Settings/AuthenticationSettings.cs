using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Settings
{
    public class GoogleSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class FacebookSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class InstagramSettings : FacebookSettings
    {
    }
}
