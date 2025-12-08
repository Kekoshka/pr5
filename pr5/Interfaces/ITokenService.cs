using pr5.Models.DTO;

namespace pr5.Interfaces
{
    public interface ITokenService
    {
        Task<string> Connect(UserCredentials userCredentials);
        ConnectionInfoDTO GetConnectionInfo(string login, string token);
        void AddUserToBlacklist(string login);
        void Disconnect(string login, string token);
    }
}
