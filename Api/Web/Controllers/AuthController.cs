using Core.Common.Api.Authentication;
using Core.Common.Api.Registration;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IRegistrationService registrationService;

        public AuthController(IAuthService authService, IRegistrationService registrationService)
        {
            this.authService = authService;
            this.registrationService = registrationService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationDto authDto)
        {
            try
            {
                var user = await authService.AuthenticateAsync(authDto);
                
                return Ok(user);
            }
            catch (AuthenticationException)
            {
                return Forbid();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationDto registrationDto)
        {
            try
            {
                var user = await registrationService.RegisterAsync(registrationDto);

                return Ok(user);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}