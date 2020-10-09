using Core.Common.Api.Password;

namespace Core.Utils.Interfaces
{
    public interface IPasswordUtils
    {
        SecuredPassword CreateHash(string password);
        bool VerifyHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}