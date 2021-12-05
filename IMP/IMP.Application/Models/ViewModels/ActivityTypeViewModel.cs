using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class ActivityTypeViewModel : BaseViewModel<int>
    {
        public string Name { get; set; }
    }

    public class ActivityTypeBasicInfoViewModel
    {
        public string Name { get; set; }
    }
}
