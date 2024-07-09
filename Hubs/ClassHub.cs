using Microsoft.AspNetCore.SignalR;

namespace EFS_23298_23327.Hubs
{
    public class ClassHub:Hub
    {
        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override async Task OnConnectedAsync() {
            if (this.Context.User.IsInRole("Cliente")){
                await Groups.AddToGroupAsync(Context.ConnectionId, "Clientes");
                await base.OnConnectedAsync();
            }
            if (this.Context.User.IsInRole("Anfitriao")) {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Anfitrioes");
                await base.OnConnectedAsync();
            }

        }
    }
}
