using System.Net;

namespace pr5.Models.DTO
{
    public class ConnectionInfo
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public IPAddress UserIp { get; set; }
    }
}
