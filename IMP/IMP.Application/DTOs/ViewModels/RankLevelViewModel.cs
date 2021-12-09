using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class RankLevelViewModel : BaseViewModel<int>
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }

    public class RankingViewModel : BaseViewModel<int>
    {
        public int Score { get; set; }
        public RankLevelViewModel RankLevel { get; set; }
    }
}
