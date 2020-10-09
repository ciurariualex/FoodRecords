using Core.Common.Api.Validation;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> BuildTokenAsync(string userName);
        Task<ValidationResult> ValidateTokenAsync(string token);
    }
}