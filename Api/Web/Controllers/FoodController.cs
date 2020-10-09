using Core.Common.Api.Food;
using Core.Data.Entities;
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
    public class FoodController : ControllerBase
    {
        private readonly IFoodService foodService;

        public FoodController(IFoodService foodService)
        {
            this.foodService = foodService;
        }

        [HttpGet]
        [Route("api/foods")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await foodService.GetAllAsync<FoodView>();
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
        [Route("api/foods/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await foodService.GetByIdAsync<FoodView>(id);
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
        [Route("api/foods/ids")]
        public async Task<IActionResult> GetByIdsAsync(ICollection<Guid> ids)
        {
            try
            {
                var result = await foodService.GetByIdsAsync<FoodView>(ids);
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
        [Route("api/foods")]
        public async Task<IActionResult> UpdateAsync([FromBody] FoodEdit dto)
        {
            try
            {
                await foodService.UpdateAsync(dto);

                return Ok();
            }
            catch (EntityNotFoundException<Food>)
            {
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/foods/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await foodService.DeleteAsync(id);

                return Ok();
            }
            catch (EntityNotFoundException<Food>)
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