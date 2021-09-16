using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class PageViewModel : BaseViewModel<int>
    {
        public int InfluencerId { get; set; }
        public string Title { get; set; }
        public string BackgroundPhoto { get; set; }
        public int PositionPage { get; set; }
    }
}
