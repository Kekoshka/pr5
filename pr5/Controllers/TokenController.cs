using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using pr5.Interfaces;
using pr5.Models.DTO;

namespace pr5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> Connect(UserCredentials userCredentials)
        {
            _tokenService.Connect(userCredentials);
        }
    }
}
