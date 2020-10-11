using Core.Common.Api.Food;
using Core.Data.Entities;
using Core.Utils.Automapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFoodService : IBaseService<Food, Guid>
    {
        Task<ICollection<TDto>> GetAllAsync<TDto>()
             where TDto : class, IFoodViewable;

        Task<TDto> GetByIdAsync<TDto>(Guid id)
            where TDto : class, IFoodViewable;

        Task<IEnumerable<TDto>> GetByIdsAsync<TDto>(IEnumerable<Guid> ids)
             where TDto : class, IFoodViewable;
        Task CreateAsync(FoodManage dto);
        
        Task UpdateAsync(FoodEdit dto);

        Task DeleteAsync(Guid id);
    }
}
