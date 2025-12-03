using System.Net;

namespace pr5.Models.DTO
{
    public class ConnectionInfoDTO
    {
        public string Token { get; set; }
        public DateTime ConnectionStart { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
