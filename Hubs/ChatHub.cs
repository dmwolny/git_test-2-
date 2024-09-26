using Microsoft.AspNetCore.SignalR;

namespace Van_Authentication.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task NewCallReceived(string textbox, string message, string group)
        {
            await Clients.Group(group).SendAsync("ReceiveCall", textbox, message);
        }

        public async Task JoinCallGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

    }
}
