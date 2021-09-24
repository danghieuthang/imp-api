using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces
{
    public interface IVnPayService
    {
        /// <summary>
        /// Create payment url
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="walletId"></param>
        /// <param name="paymentInfo"></param>
        /// <param name="ipAddress"></param>
        /// <param name="locale">vn|en</param>
        /// <returns></returns>
        public string CreatePaymentUrl(int amount, int walletId, string paymentInfo, string locale="vn");
    }
}
