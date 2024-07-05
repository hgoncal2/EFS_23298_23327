using Microsoft.AspNetCore.SignalR;

namespace EFS_23298_23327.Hubs
{
    public class ClassHub:Hub
    {
        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
