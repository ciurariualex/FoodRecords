using Core.Context;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public abstract class BaseEntityService<TEntity> : IBaseEntityService<TEntity>
        where TEntity : class
    {
        private readonly CrudApiContext context;

        public BaseEntityService(CrudApiContext context) => this.context = context;

        public async virtual Task<List<TEntity>> GetAllAsync()
        {
            return await context
                .Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();
        }

        public IQueryable<TEntity> GetQueriable(bool withTrack = false)
        {
            return withTrack
                ? context.Set<TEntity>()
                : context.Set<TEntity>().AsNoTracking();
        }

        public async virtual Task<int> GetCountAsync()
        {
            return await context.Set<TEntity>().CountAsync();
        }

        public async virtual Task<long> GetLongCountAsync()
        {
            return await context.Set<TEntity>().LongCountAsync();
        }

        public async virtual Task CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
            await SaveAsync();
        }

        public virtual async Task CreateBulkAsync(ICollection<TEntity> entities)
        {
            await context.Set<TEntity>().AddRangeAsync(entities);
            await SaveAsync();
        }

        public async virtual Task CreateAsync(List<TEntity> entities)
        {
            await context.Set<TEntity>().AddRangeAsync(entities);
            await SaveAsync();
        }

        public async virtual Task UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await SaveAsync();
        }

        public virtual async Task UpdateBulkAsync(ICollection<TEntity> entities)
        {
            context.Set<TEntity>().UpdateRange(entities);
            await SaveAsync();
        }

        public async virtual Task UpdateAsync(List<TEntity> entities)
        {
            context.Set<TEntity>().UpdateRange(entities);
            await SaveAsync();
        }

        public async virtual Task DeleteAsync(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            await SaveAsync();
        }

        public virtual async Task DeleteBulkAsync(ICollection<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
            await SaveAsync();
        }

        public async virtual Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}