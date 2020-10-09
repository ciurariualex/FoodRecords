using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBaseEntityService<TEntity>
        where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetQueriable(bool withTrack = false);
        Task<int> GetCountAsync();
        Task<long> GetLongCountAsync();
        Task CreateAsync(TEntity entity);
        Task CreateBulkAsync(ICollection<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateBulkAsync(ICollection<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteBulkAsync(ICollection<TEntity> entities);
        Task SaveAsync();
    }
}