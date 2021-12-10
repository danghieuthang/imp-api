using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using IMP.Application.Models.Compaign;

namespace IMP.Application.Models.ViewModels
{
    public class BaseVoucherViewModel : BaseViewModel<int>
    {
        public int BrandId { get; set; }
        public string VoucherName { get; set; }
        public string Code { get; set; }
        public bool OnlyforInfluencer { get; set; }
        public bool OnlyforCustomer { get; set; }
        public decimal DiscountValue { get; set; }
        public int DiscountValueType { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public string Image { get; set; }
        public string Thumnail { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }
        public bool IsCanUse => QuantityUsed > Quantity;
        public TimeSpan? HoldTime { get; set; }
    }
    public class VoucherViewModel : BaseViewModel<int>
    {
        public int BrandId { get; set; }
        public string VoucherName { get; set; }
        public string Code { get; set; }
        public bool OnlyforInfluencer { get; set; }
        public bool OnlyforCustomer { get; set; }
        public decimal DiscountValue { get; set; }
        public int DiscountValueType { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public string Image { get; set; }
        public string Thumnail { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }
        public bool IsCanUse => QuantityUsed > Quantity;
        public TimeSpan? HoldTime { get; set; }

        public List<VoucherCodeViewModel> VoucherCodes { get; set; }
        public List<CampaignVoucherOnlyIdViewModel> CampaignVouchers { get; set; }
        public List<DiscountProductViewModel> DiscountProducts { get; set; }
        public BrandViewModel Brand { get; set; }
    }

    public class VoucherReportViewModel : VoucherViewModel
    {
        public int QuantityGet { get; set; }
    }

    public class VoucherCodeViewModel : BaseViewModel<int>
    {

        public string Code { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public int? CampaignMemberId { get; set; }
    }

    public class TransactionVoucherCodeViewModel : BaseViewModel<int>
    {

        public string Code { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public int? CampaignMemberId { get; set; }
        public BaseVoucherViewModel Voucher { get; set; }
    }

    public class CheckVoucherCodeViewModel
    {

        public string Code { get; set; }
        public int Quantity { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime? Expired { get; set; }
        public int InfluencerId { get; set; }
        public int CampaignId { get; set; }
        public int VoucherId { get; set; }
    }

    public class UserVoucherViewModel : BaseViewModel<int>
    {
        public int VoucherType { get; set; }
        public string VoucherName { get; set; }
        public string Code { get; set; }
        public bool OnlyforInfluencer { get; set; }
        public bool OnlyforCustomer { get; set; }
        public decimal DiscountValue { get; set; }
        public int DiscountValueType { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public int QuantityUsed { get; set; }
        public string Image { get; set; }
        public string Thumnail { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Condition { get; set; }
        public string Target { get; set; }

        public bool IsCanUse => QuantityUsed > Quantity;
        public TimeSpan? HoldTime { get; set; }

        public BrandViewModel Brand { get; set; }
        public List<UserVoucherCodeViewModel> VoucherCodes { get; set; }
        public List<CampaignVoucherViewModel> CampaignVouchers { get; set; }
        public List<DiscountProductViewModel> DiscountProducts { get; set; }
    }

    public class UserVoucherCodeViewModel : BaseViewModel<int>
    {

        public string Code { get; set; }
        [JsonIgnore]
        public int Quantity { get; set; }
        [JsonIgnore]
        public int QuantityUsed { get; set; }
        public TimeSpan? HoldTime { get; set; }

        public bool IsCanUse => QuantityUsed < Quantity;
    }

    public class UserCampaignVoucherViewModel : BaseViewModel<int>
    {
        public int CampaignId { get; set; }
        public VoucherViewModel Voucher { get; set; }
    }
    public class VoucherTransactionViewModel : BaseViewModel<int>
    {
        public decimal TotalProductAmount { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int ProductQuantity { get; set; }
        public Order Order { get; set; }
        public string Status { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderCreated { get; set; }
        public decimal EarningMoney { get; set; }
        public TransactionVoucherCodeViewModel VoucherCode { get; set; }
        public InfluencerViewModel Influencer { get; set; }
    }

    public class VoucherTransactionReportViewModel
    {
        public int TotalTransaction { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalProductAmount { get; set; }
        public int TotalVoucherCode { get; set; }
        public decimal TotalEarningAmount { get; set; }
    }

    public class VoucherTransactionReportOfVoucherViewModel : VoucherTransactionReportViewModel
    {
        public int VoucherId { get; set; }
        public string Name { get; set; }
        public int QuantityGet { get; set; }
        public int QuantityUsed { get; set; }
        public int TotalVoucherCodeQuantity { get; set; }
    }


    public class DiscountProductViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class DiscountProductRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class RangeVoucherDateViewModel
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }

    public class VoucherTransactionRequest
    {
        public int CampaignId { get; set; }
        [JsonProperty("voucher_code")]
        public string Code { get; set; }
        public decimal TotalProductAmount { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int ProductQuantity { get; set; }
        public Order Order { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderCreated { get; set; }
    }

    public class Order
    {
        public string OrderCode { get; set; }
        public DateTime Created { get; set; }
        public string BrandName { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public List<OrderLine> Lines { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal FinalTotalAmount { get; set; }
    }

    public class OrderLine
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}