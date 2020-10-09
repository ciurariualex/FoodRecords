using AutoMapper;
using Core.Common.Api.Registration;
using Core.Context;
using Core.Data.Entities.Identity;
using Core.Interfaces;
using Core.Utils.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly CrudApiContext context;
        private readonly IMapper mapper;
        private readonly IApplicationUserService userService;
        private readonly IPasswordUtils passwordUtils;

        public RegistrationService(
            CrudApiContext context,
            IMapper mapper,
            IApplicationUserService userService,
            IPasswordUtils passwordUtils)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
            this.passwordUtils = passwordUtils;
        }

        public async Task<RegistrationView> RegisterAsync(RegistrationDto dto)
        {
            var existingUser = await context.Users.SingleOrDefaultAsync(auth => auth.Username == dto.Username);
            if (existingUser != null)
            {
                throw new AuthenticationException();
            }

            var user = mapper.Map<ApplicationUser>(dto);

            var securedPassword = passwordUtils.CreateHash(dto.Password);
            user.SetHashAndSalt(securedPassword.Hash, securedPassword.Salt);

            await userService.CreateAsync(user);

            return mapper.Map<RegistrationView>(user);
        }
    }
}