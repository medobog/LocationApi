using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Location.Infrastructure;

namespace Location
{
    public class Informacije : Hub
    {
        public async Task Send(InfoModel model)
        {
            await Clients.All.SendAsync("Send", JsonConvert.SerializeObject(model));
        }
    }
}