using Core.Common.Api.User;
using Core.Data.Entities.Identity;
using Core.Data.Enums;
using Core.Interfaces;
using Core.Utils.Exceptions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserService applicationUserService;

        public UserController(IApplicationUserService applicationUserService)
        {
            this.applicationUserService = applicationUserService;
        }

        [HttpGet]
        [Route("api/users")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await applicationUserService.GetAllAsync<UserView>();
                if (!result.Any())
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/users/roles")]
        public async Task<IActionResult> GetByRolesAsync(ICollection<Role> roles)
        {
            try
            {
                var result = await applicationUserService.GetByRolesAsync<UserView>(roles);
                if (!result.Any())
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/users/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await applicationUserService.GetByIdAsync<UserView>(id);
                if (result is null)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/users/ids")]
        public async Task<IActionResult> GetByIdsAsync(ICollection<Guid> ids)
        {
            try
            {
                var result = await applicationUserService.GetByIdsAsync<UserView>(ids);
                if (!result.Any())
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/users")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserEdit dto)
        {
            try
            {
                await applicationUserService.UpdateAsync(dto);

                return Ok();
            }
            catch (EntityNotFoundException<ApplicationUser>)
            {
                return NoContent();
            }
            catch (PasswordMismatchException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/users/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await applicationUserService.DeleteAsync(id);

                return Ok();
            }
            catch (EntityNotFoundException<ApplicationUser>)
            {
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}