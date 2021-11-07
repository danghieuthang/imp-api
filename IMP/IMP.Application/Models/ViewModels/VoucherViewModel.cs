using System.Collections.Generic;
using System;

namespace IMP.Application.Models.ViewModels
{
    public class VoucherViewModel : BaseViewModel<int>
    {
        public int BrandId { get; set; }
        public string VoucherName { get; set; }
        public string Image { get; set; }
        public string Thumnail { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }

        public List<VoucherCodeViewModel> VoucherCodes { get; set; }
        public List<CampaignVoucherViewModel> CampaignVouchers { get; set; }
    }

    public class VoucherCodeViewModel : BaseViewModel<int>
    {

        public string Code { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
    }

    public class VoucherTransactionViewModel : BaseViewModel<int>
    {
        public int VoucherCodeId { get; set; }
    }
}