using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class PlatformViewModel : BaseViewModel<int>
    {
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
