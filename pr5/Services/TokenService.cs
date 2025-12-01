using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using pr5.Context;
using pr5.Interfaces;
using pr5.Models.DTO;
using System.Threading.Tasks;

namespace pr5.Services
{
    public class TokenService : ITokenService
    {
        IHttpContextAccessor _httpContextAccessor;
        ApplicationContext _context;
        IMemoryCache _cache;
        public TokenService(
            IHttpContextAccessor httpContextAccessor,
            ApplicationContext context,
            IMemoryCache cache) 
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _cache = cache;
        }
        public void CreateToken()
        {

        }
        public void AddUserToBlacklist()
        {

        }

        public async Task Connect(UserCredentials userCredentials)
        {
            if (!await IsExistsUser(userCredentials)) throw new UnauthorizedAccessException();

            
        }
        private async Task<bool> IsExistsUser(UserCredentials userCredentials)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.Login == userCredentials.Login && u.Password == userCredentials.Password);
        }
    }
}
