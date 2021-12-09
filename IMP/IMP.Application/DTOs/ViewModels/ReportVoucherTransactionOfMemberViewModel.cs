using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class ReportVoucherTransactionOfMemberViewModel
    {
        public DateTime Date { get; set; }
        public int TotalVoucherTransaction { get; set; }
        public decimal TotalMoneyEarning { get; set; }
        public decimal TotalProductAmount { get; set; }
        public int TotalProductQuantity { get; set; }
    }
}
