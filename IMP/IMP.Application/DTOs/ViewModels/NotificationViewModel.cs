using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Models.ViewModels
{
    public class NotificationViewModel : BaseViewModel<int>
    {
        public int ApplicationUserId { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public int RedirectId { get; set; }
        public bool IsRead { get; set; }
    }

    public class CountUnreadNotification
    {
        public int Number { get; set; }
    }
}
