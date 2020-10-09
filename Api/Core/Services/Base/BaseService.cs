using Core.Context;
using Core.Data.Entities.Base;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public abstract class BaseService<TEntity, TId> : BaseEntityService<TEntity>, IBaseService<TEntity, TId>
        where TEntity : class, IEntity<TId>, IDeletable, IStableEntity
        where TId : struct
    {
        private readonly CrudApiContext context;
        private readonly Guid userId = Guid.Empty;

        public BaseService(CrudApiContext context, IHttpContextAccessor httpContextAccessor)
           : base(context)
        {
            this.context = context;

            var name = httpContextAccessor.HttpContext.User.Identity.Name;
            userId = name is null ? Guid.Empty : Guid.Parse(name);
        }

        public async virtual Task<TEntity> GetByIdAsync(TId id)
        {
            return await context
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public async virtual Task<List<TEntity>> GetByIdsAsync(List<TId> ids)
        {
            return await context
                .Set<TEntity>()
                .AsNoTracking()
                .Join(
                    ids,
                    entity => entity.Id,
                    id => id,
                    (entity, id) => entity)
                .ToListAsync();
        }

        public override async Task CreateAsync(TEntity entity)
        {
            entity.CreatedBy = userId;

            await base.CreateAsync(entity);
        }
        public override async Task CreateBulkAsync(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = userId;
            }

            await base.CreateBulkAsync(entities);
        }

        public override async Task UpdateAsync(TEntity entity)
        {
            entity.UpdatedBy = userId;
            entity.UpdatedAt = DateTime.UtcNow;

            await base.UpdateAsync(entity);
        }

        public override async Task UpdateBulkAsync(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdatedBy = userId;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            await base.UpdateBulkAsync(entities);
        }


        public override async Task DeleteAsync(TEntity entity)
        {
            entity.SoftDelete();
            await UpdateAsync(entity);
        }

        public override async Task DeleteBulkAsync(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.SoftDelete();
            }

            await UpdateBulkAsync(entities);
        }
    }
}
