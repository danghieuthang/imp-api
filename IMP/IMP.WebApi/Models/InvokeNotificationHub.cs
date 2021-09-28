using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace IMP.WebApi.Models
{
    public interface IInvokeNotificationHub
    {
        Task SendNotification(string groupId, string message);
    }

    public class InvokeNotificationHub : IInvokeNotificationHub
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public InvokeNotificationHub(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(string groupId, string message)
        {
            await _hubContext.Clients.Group(groupId).SendAsync("notification", message);
        }
    }
}