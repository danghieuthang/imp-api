using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class PageViewModel : BaseViewModel<int>
    {
        public string BackgroundType { get; set; }
        public double FontSize { get; set; }
        public string Background { get; set; }
        public string BioLink { get; set; }
        public string FontFamily { get; set; }
        public string TextColor { get; set; }
        public List<BlockViewModel> Blocks { get; set; }
        public int Status { get; set; }
    }
}
