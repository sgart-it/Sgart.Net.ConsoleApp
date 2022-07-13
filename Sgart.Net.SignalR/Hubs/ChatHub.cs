using Microsoft.AspNetCore.SignalR;
using Sgart.Net.SignalR.DTO;

namespace Sgart.Net.SignalR.Hubs
{
    /// <summary>
    /// gestisce la comunicazione client/server
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// il nome del metodo è il valore da usare 
        /// lato JavaScript connection.invoke("SendMessage", data)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendMessage(ChatDTO data)
        {
            // applico un timestamp certo
            data.Date = DateTime.UtcNow;

            await Clients.All.SendAsync("ReceiveMessage", data);
        }
    }
}
