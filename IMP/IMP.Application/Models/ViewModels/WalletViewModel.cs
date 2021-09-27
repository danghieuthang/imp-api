using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class WalletViewModel : BaseViewModel<int>
    {
        public decimal Balance { get; set; }
        //public List<WalletTransactionViewModel> WalletTransactions { get; set; }
    }

    public class ChargeWalletRequest
    {
        public int Amount { get; set; }
    }

    public class ChargeWalletResponse
    {
        public string Url { get; set; }
    }
}
