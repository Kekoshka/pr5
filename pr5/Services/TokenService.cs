using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using pr5.Context;
using pr5.Hubs;
using pr5.Interfaces;
using pr5.Models.DTO;
using pr5.Common.CustomExceptions;
using System.Linq;
using System.Threading.Tasks;
using ConnectionInfo = pr5.Models.DTO.ConnectionInfo;

namespace pr5.Services
{
    public class TokenService : ITokenService
    {
        IHttpContextAccessor _httpContextAccessor;
        ApplicationContext _context;
        IMemoryCache _cache;
        IHubContext<MainHub> _hub;
        private int TokenLength = 10;
        public TokenService(
            IHttpContextAccessor httpContextAccessor,
            ApplicationContext context,
            IMemoryCache cache,
            IHubContext<MainHub> hub) 
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _cache = cache;
            _hub = hub;
        }

        public void AddUserToBlacklist(string login)
        {
            _cache.Set("Blacklist_" + login, login);
            var isGet = _cache.TryGetValue("Connection_" + login, out ConnectionInfo connection);
            if (!isGet)
            {
                Console.WriteLine("Пользователь не найден");
                return;
            }
            DisconnectUser(login, connection.ConnectionId);
        }
        public void Disconnect(string login, string token)
        {
            var isGet = _cache.TryGetValue("Connection_" + login, out ConnectionInfo connection);
            if (!isGet)
                throw new NotFoundException();

            if (connection.Token == token)
                DisconnectUser(login, connection.ConnectionId);
        }
        public ConnectionInfoDTO GetConnectionInfo(string login, string token)
        {
            var isGet = _cache.TryGetValue("Connection_" + login, out ConnectionInfo connection);
            if (!isGet)
                throw new NotFoundException();
            
            if (connection.Token != token) throw new ForbiddenException();
            return new ConnectionInfoDTO()
            {
                ConnectionStart = connection.ConnectionStart,
                Duration = DateTime.UtcNow - connection.ConnectionStart,
                Token = token
            };         
        }

        public async Task<string> Connect(UserCredentials userCredentials)
        {
            if (!await IsExistsUser(userCredentials)) throw new UnauthorizedException();
            if (IsUserAlreadyConnect(userCredentials.Login)) throw new ConflictException();

            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            var token = CreateToken();

            ConnectionInfo connectionInfo = new()
            {
                Login = userCredentials.Login,
                Password = userCredentials.Password,
                Token = token,
                UserIp = clientIp,
                ConnectionStart = DateTime.UtcNow
            };

            _cache.Set("Connection_" + userCredentials.Login,connectionInfo);
            return token;
        }

        private async Task<bool> IsExistsUser(UserCredentials userCredentials)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.Login == userCredentials.Login && u.Password == userCredentials.Password);
        }
        private bool IsUserAlreadyConnect(string login)
        {
            return _cache.TryGetValue("Connection_" + login, out object connectionInfo);
        }
        private void DisconnectUser(string login, string connectionId)
        {
            _cache.Remove("Connection_" + login);
            _hub.Clients.Client(login).SendAsync("disconnect");
        }
        private string CreateToken()
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm".ToCharArray();
            var rndChar = new Random().Next(0, chars.Length);
            var rndDigits = new Random().Next(0, 9);
            var rndBool = new Random().Next(0, 1);
            string token = string.Empty;

            for (int i = 0; i < TokenLength; i++)
                token += rndBool == 0 ? chars[rndChar] : rndDigits.ToString();

            return token;
        }

    }
}
