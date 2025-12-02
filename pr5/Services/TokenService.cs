using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using pr5.Context;
using pr5.Interfaces;
using pr5.Models.DTO;
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
        private int TokenLength = 10
        public TokenService(
            IHttpContextAccessor httpContextAccessor,
            ApplicationContext context,
            IMemoryCache cache) 
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _cache = cache;
        }
        private string CreateToken()
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm".ToCharArray();
            var rndChar = new Random().Next(0, chars.Length);
            var rndDigits = new Random().Next(0, 9);
            var rndBool = new Random().Next(0, 1);
            string token = string.Empty;

            for (int i = 0; i < TokenLength; i++)
                token += rndBool == 0 ? rndChar : rndDigits.ToString();
            
            return token;

        }
        public void AddUserToBlacklist()
        {

        }

        public async Task<string> Connect(UserCredentials userCredentials)
        {
            if (!await IsExistsUser(userCredentials)) throw new UnauthorizedAccessException();

            var clientIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            var token = CreateToken();

            ConnectionInfo connectionInfo = new()
            {
                Login = userCredentials.Login,
                Password = userCredentials.Password,
                Token = token,
                UserIp = clientIp
            };
            _cache.Set(userCredentials.Login, connectionInfo);
        }
        public async Task ConnectByToken()
        {

        }
        private async Task<bool> IsExistsUser(UserCredentials userCredentials)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.Login == userCredentials.Login && u.Password == userCredentials.Password);
        }
    }
}
