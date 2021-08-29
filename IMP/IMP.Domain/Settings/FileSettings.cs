using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Domain.Settings
{
    public class FileSettings
    {
        public int MaximumSize { get; set; }
        public IEnumerable<string> AllowTypes { get; set; }
    }
}
