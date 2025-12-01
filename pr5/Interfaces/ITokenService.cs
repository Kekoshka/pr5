using pr5.Models.DTO;

namespace pr5.Interfaces
{
    public interface ITokenService
    {
        void Connect(UserCredentials userCredentials);
    }
}
