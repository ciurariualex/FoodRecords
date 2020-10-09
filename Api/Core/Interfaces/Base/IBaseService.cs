using Core.Data.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBaseService<TEntity, TId> : IBaseEntityService<TEntity>
        where TEntity : class, IEntity<TId>, IDeletable, IStableEntity
        where TId : struct
    {
        Task<TEntity> GetByIdAsync(TId id);
        Task<List<TEntity>> GetByIdsAsync(List<TId> ids);
    }
}