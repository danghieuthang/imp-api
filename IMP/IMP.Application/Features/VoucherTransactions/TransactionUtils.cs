//using IMP.Application.Enums;
//using IMP.Application.Models.ViewModels;
//using IMP.Domain.Entities;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IMP.Application.Features.VoucherTransactions
//{
//    public class TransactionUtils
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="campaign"></param>
//        /// <param name="transaction"></param>
//        /// <returns></returns>
//        public static decimal CaculateMoneyEarnFromTransaction(Campaign campaign, VoucherTransaction transaction)
//        {
//            decimal earingMoney = 0;

//            List<VoucherCommissionPrices> prices = JsonConvert.DeserializeObject<List<VoucherCommissionPrices>>(campaign.VoucherCommissionPrices);
//            if (campaign.VoucherCommissionMode == (int)VoucherCommissionType.Order) // if earning per oder
//            {
//                if (campaign.IsPercentVoucherCommission) // if earn by percent of order
//                {
//                    earingMoney = prices.FirstOrDefault().Value * transaction.TotalPrice / (decimal)100.0;
//                }
//                else // earn by interval of order
//                {
//                    if (prices.Count > 0)
//                    {
//                        foreach (var price in prices)
//                        {
//                            if (transaction.TotalPrice >= price.From && (transaction.TotalPrice <= price.To || price.To == null))
//                            {
//                                earingMoney = price.Value;
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//            else if (campaign.VoucherCommissionMode == (int)VoucherCommissionType.Product)// if earning by product
//            {
//                if (campaign.IsPercentVoucherCommission) // if earn by percent
//                {
//                    if (prices.Count > 0)
//                    {
//                        earingMoney = prices.FirstOrDefault().Value * transaction.TotalDiscount / (decimal)100.0;
//                    }
//                }
//            }

//            return earingMoney;
//        }
//    }
//}
