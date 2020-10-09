using AutoMapper;
using Core.Common.Api.Authentication;
using Core.Common.Api.Validation;
using Core.Context;
using Core.Data.Entities.Identity;
using Core.Interfaces;
using Core.Utils.Exceptions;
using Core.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public sealed class AuthService : BaseService<ApplicationUser, Guid>, IAuthService
    {
        private readonly CrudApiContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IPasswordUtils passwordUtils;
        private readonly ITokenService tokenService;

        public AuthService(
            CrudApiContext context,
            IMapper mapper,
            IConfiguration configuration,
            IPasswordUtils passwordUtils,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.passwordUtils = passwordUtils;
            this.tokenService = tokenService;
        }


        public async Task PasswordResetAsync(ValidationDto dto)
        {
            var validationResult = await tokenService.ValidateTokenAsync(dto.Token);

            if (!validationResult.IsValid)
            {
                throw new SecurityTokenException();
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                throw new PasswordMismatchException("Passwords are different!");
            }

            var user = await context.Users.SingleOrDefaultAsync(auth => auth.Id == validationResult.User.Id);

            if (user is null)
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            if (!passwordUtils.VerifyHash(dto.OldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            var securedPassword = passwordUtils.CreateHash(dto.NewPassword);

            user.SetHashAndSalt(securedPassword.Hash, securedPassword.Salt);

            await UpdateAsync(user);
        }

        public async Task<AuthenticationView> AuthenticateAsync(AuthenticationDto dto)
        {
            var authUser = await context.Users.SingleOrDefaultAsync(auth => auth.Username == dto.Username);

            if (authUser is null)
            {
                throw new AuthenticationException();
            }

            if (!passwordUtils.VerifyHash(dto.Password, authUser.PasswordHash, authUser.PasswordSalt))
            {
                throw new AuthenticationException();
            }

            var userAuthenticateView = mapper.Map<AuthenticationView>(authUser);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Auth:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userAuthenticateView.Id.ToString()),
                    new Claim(ClaimTypes.Role, authUser.Role.ToString())
                },
                authUser.Role.ToString()),
                Expires = DateTime.UtcNow.AddHours(int.TryParse(configuration["Auth:TokenAvailability"], out int result) ? result : 24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userAuthenticateView.Token = tokenHandler.WriteToken(token);

            return userAuthenticateView;
        }
    }
}
