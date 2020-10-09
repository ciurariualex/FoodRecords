using Core.Common.Api.Password;
using Core.Utils.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Core.Utils.Services
{
    public class PasswordUtils : IPasswordUtils
    {
        private readonly IConfiguration configuration;

        public PasswordUtils(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SecuredPassword CreateHash(string password)
        {
            SecuredPassword securedPassword = new SecuredPassword();

            using (var hmac = new HMACSHA512())
            {
                securedPassword.Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                securedPassword.Salt = hmac.Key;
            }

            return securedPassword;
        }

        public bool VerifyHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}