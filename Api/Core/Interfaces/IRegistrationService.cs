using Core.Common.Api.Registration;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRegistrationService
    {
        Task<RegistrationView> RegisterAsync(RegistrationDto dto);
    }
}
