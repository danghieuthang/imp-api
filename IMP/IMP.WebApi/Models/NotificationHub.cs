using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace IMP.WebApi.Models
{
    [Authorize]
    public class NotificationHub : DynamicHub
    {
        private readonly static Dictionary<string, string> _connection = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName);
            await base.OnDisconnectedAsync(exception);
        }
        private string GetGroupName => Context.User.FindFirst("uid").Value;
    }
}