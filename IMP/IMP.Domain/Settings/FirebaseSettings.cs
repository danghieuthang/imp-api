using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Settings
{
    public class FirebaseSettings
    {
        public string ApiKey { get; set; }
        public string AuthDomain { get; set; }
        public string StorageBucket { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
