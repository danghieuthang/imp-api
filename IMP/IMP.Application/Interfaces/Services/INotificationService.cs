using IMP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMP.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task PutNotication(int applicationUserid, int redirectId, NotificationType notificationType);
    }
}
