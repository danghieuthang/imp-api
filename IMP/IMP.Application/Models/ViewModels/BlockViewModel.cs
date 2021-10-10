using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMP.Application.Models.Compaign;

namespace IMP.Application.Models.ViewModels
{
    public class BlockViewModel : BaseViewModel<int>
    {
        public int BlockTypeId { get; set; }
        public string Variant { get; set; }
        public int Position { get; set; }
        public bool Enable { get; set; }
        public dynamic Data { get; set; }
        public List<BlockViewModel> ChildBlocks { get; set; }
    }

}
