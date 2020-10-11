using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Common.Api.Food;
using Core.Context;
using Core.Data.Entities;
using Core.Interfaces;
using Core.Services;
using Core.Utils.Automapper.Interfaces;
using Core.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBOS.Core.Services
{
    public sealed class FoodService : BaseService<Food, Guid>, IFoodService
    {
        private readonly CrudApiContext context;
        private readonly IMapper mapper;

        public FoodService(
            CrudApiContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ICollection<TDto>> GetAllAsync<TDto>()
            where TDto : class, IFoodViewable
        {
            return await context
                .Foods
                .AsNoTracking()
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDto> GetByIdAsync<TDto>(Guid id)
            where TDto : class, IFoodViewable
        {
            return await context
                .Foods
                .AsNoTracking()
                .Where(user => user.Id.Equals(id))
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDto>> GetByIdsAsync<TDto>(IEnumerable<Guid> ids)
             where TDto : class, IFoodViewable
        {
            return await context
                .Foods
                .AsNoTracking()
                .Join(ids, food => food.Id, id => id, (food, _) => food)
                .Distinct()
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDto> CreateAsync<TDto>(FoodManage dto)
            where TDto : class, IFoodViewable
        {
            var food = mapper.Map<Food>(dto);

            await CreateAsync(food);

            return mapper.Map<TDto>(food);
        }

        public async Task CreateAsync(FoodManage dto)
        {
            var food = mapper.Map<Food>(dto);

            await CreateAsync(food);
        }

        public async Task UpdateAsync(FoodEdit dto)
        {
            var food = await GetByIdAsync(dto.Id);
            if (food is null)
            {
                throw new EntityNotFoundException<Food>();
            }

            mapper.Map(dto, food);

            await UpdateAsync(food);
        }

        public async Task DeleteAsync(Guid id)
        {
            var food = await GetByIdAsync(id);
            if (food is null)
            {
                throw new EntityNotFoundException<Food>();
            }

            await DeleteAsync(food);
        }
    }
}