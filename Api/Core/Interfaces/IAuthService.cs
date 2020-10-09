using Core.Common.Api.Authentication;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationView> AuthenticateAsync(AuthenticationDto authDto);
    }
}
