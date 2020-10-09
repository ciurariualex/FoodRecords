using Core.Common.Api.User;
using Core.Common.Api.Validation;
using Core.Data.Entities.Identity;
using Core.Interfaces;
using Core.Utils.Exceptions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IApplicationUserService userService;

        public TokenService(IApplicationUserService userService)
        {
            this.userService = userService;
        }

        public async Task<string> BuildTokenAsync(string userName)
        {
            var result = await userService.GetByUserNameAsync<UserView>(userName);

            if (result is null)
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());

            Encoding encodingUTF8 = Encoding.UTF8;
            byte[] Id = encodingUTF8.GetBytes(result.Id.ToString());

            byte[] data = new byte[time.Length + Id.Length];

            Buffer.BlockCopy(time, 0, data, 0, time.Length);
            Buffer.BlockCopy(Id, 0, data, time.Length, Id.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        public async Task<ValidationResult> ValidateTokenAsync(string token)
        {
            ValidationResult validationResult = new ValidationResult();

            byte[] data = Convert.FromBase64String(token);

            byte[] tokenTime = data.Take(8).ToArray();
            byte[] tokenId = data.Skip(8).Take(36).ToArray();

            DateTime tokenMade = DateTime.FromBinary(BitConverter.ToInt64(tokenTime, 0));

            if (tokenMade < DateTime.UtcNow.AddHours(-24))
            {
                validationResult.IsValid = false;

                return validationResult;
            }

            var userId = Guid.Parse(Encoding.UTF8.GetString(tokenId, 0, tokenId.Length));
            var user = await userService.GetByIdAsync<UserView>(userId);

            if (user is null)
            {
                validationResult.IsValid = false;

                return validationResult;
            }

            validationResult.IsValid = true;
            validationResult.User = user;

            return validationResult;
        }
    }
}
