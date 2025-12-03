using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using pr5.Models.DTO;

namespace pr5.Hubs
{
    public class MainHub : Hub
    {
        IMemoryCache _cache;
        public MainHub(IMemoryCache cache) 
        {
            _cache = cache;
        }
        public void Subscribe(string login, string token)
        {
            var isGet = _cache.TryGetValue("Connection_" + login, out Models.DTO.ConnectionInfo value);
            if (!isGet)
            {
                Clients.Caller.SendAsync("error", "user with this login and token not found");
                return;
            }
            value.ConnectionId = Context.ConnectionId;
            _cache.Set("Connection_" + login, value);
        }
    }
}
