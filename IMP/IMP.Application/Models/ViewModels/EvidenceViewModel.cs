using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class EvidenceViewModel : BaseViewModel<int>
    {
        public int EvidenceTypeId { get; set; }
        public int MemberActivityId { get; set; }
        public string Url { get; set; }
    }
}
