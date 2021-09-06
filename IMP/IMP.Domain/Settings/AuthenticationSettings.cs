using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Settings
{
    public class GoogleAuthenticationSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class FacebookAuthenticationSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
