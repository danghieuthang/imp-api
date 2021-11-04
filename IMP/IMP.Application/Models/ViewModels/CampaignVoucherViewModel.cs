using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class CampaignVoucherViewModel : BaseViewModel<int>
    {
        public int CampaignId { get; set; }
        public int VoucherId { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int PercentForInfluencer { get; set; }
    }

    public class CampaignVoucherRequest
    {
        public int VoucherId { get; set; }
        public int Quantity { get; set; }
        public int? QuantityForInfluencer { get; set; }
        public int PercentForInfluencer { get; set; }
    }
}
