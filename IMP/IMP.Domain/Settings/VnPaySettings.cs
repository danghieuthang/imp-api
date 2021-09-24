using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Settings
{
    public class VnPaySettings
    {
        public string Vnp_Url { get; set; }
        public string Querydr { get; set; }
        public string Vnp_TmnCode { get; set; }
        public string Vnp_HashSecret{ get; set; }
        public string Vnp_ReturnUrl{ get; set; }
    }
}
