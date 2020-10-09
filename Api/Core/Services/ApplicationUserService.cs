using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Common.Api.User;
using Core.Context;
using Core.Data.Entities.Identity;
using Core.Data.Enums;
using Core.Interfaces;
using Core.Services;
using Core.Utils.Automapper.Interfaces;
using Core.Utils.Exceptions;
using Core.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBOS.Core.Services
{
    public sealed class ApplicationUserService : BaseService<ApplicationUser, Guid>, IApplicationUserService
    {
        private readonly CrudApiContext context;
        private readonly IMapper mapper;
        private readonly IPasswordUtils passwordUtils;

        public ApplicationUserService(
            CrudApiContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPasswordUtils passwordUtils)
            : base(context, httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.passwordUtils = passwordUtils;
        }

        public async Task<ICollection<TDto>> GetAllAsync<TDto>()
            where TDto : class, IUserViewable
        {
            return await context
                .Users
                .AsNoTracking()
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDto> GetByUserNameAsync<TDto>(string userName)
            where TDto : class, IUserViewable
        {
            return await context
                .Users
                .AsNoTracking()
                .Where(user => user.Username.Equals(userName))
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<TDto>> GetByRolesAsync<TDto>(IEnumerable<Role> roles)
            where TDto : class, IUserViewable
        {
            return await context
                .Users
                .Where(user => roles.Any(role => role == user.Role))
                .AsNoTracking()
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDto> GetByIdAsync<TDto>(Guid id)
            where TDto : class, IUserViewable
        {
            return await context
                .Users
                .AsNoTracking()
                .Where(user => user.Id.Equals(id))
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TDto>> GetByIdsAsync<TDto>(IEnumerable<Guid> ids)
             where TDto : class, IUserViewable
        {
            return await context
                .Users
                .AsNoTracking()
                .Join(ids, user => user.Id, id => id, (user, _) => user)
                .Distinct()
                .ProjectTo<TDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDto> CreateAsync<TDto>(UserAdd dto)
            where TDto : class, IUserViewable
        {
            var user = mapper.Map<ApplicationUser>(dto);

            var password = passwordUtils.CreateHash("Parola123!");

            user.SetHashAndSalt(password.Hash, password.Salt);

            await CreateAsync(user);

            return mapper.Map<TDto>(user);
        }

        public async Task<IEnumerable<TDto>> CreateBulkAsync<TDto>(ICollection<UserAdd> dtos)
            where TDto : class, IUserViewable
        {
            var users = mapper.Map<ICollection<ApplicationUser>>(dtos);

            foreach (var user in users)
            {
                var securedPassword = passwordUtils.CreateHash("Parola123!");

                user.SetHashAndSalt(securedPassword.Hash, securedPassword.Salt);
            }

            await CreateBulkAsync(users);

            return mapper.Map<IEnumerable<TDto>>(users);
        }

        public async Task UpdateAsync(UserEdit dto)
        {
            var user = await GetByIdAsync(dto.Id);
            if (user is null)
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            mapper.Map(dto, user);

            await UpdateAsync(user);
        }

        public async Task UpdateRoleAsync(Guid id, UserRoleEdit dto)
        {
            var user = await GetByIdAsync(id);
            if (user is null)
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            mapper.Map(dto, user);

            await UpdateAsync(user);
        }

        public async Task UpdateBulkAsync(ICollection<UserEdit> dtos)
        {
            var ids = dtos.Select(dto => dto.Id).Distinct().ToList();
            var userEdits = await base.GetByIdsAsync(ids);
            if (!userEdits.Any())
            {
                throw new EntityNotFoundException<UserEdit>();
            }

            foreach (var destUserEdit in userEdits)
            {
                var srcUserEdit = dtos.First(dto => destUserEdit.Id.Equals(dto.Id));
                mapper.Map(srcUserEdit, destUserEdit);
            }

            await UpdateBulkAsync(userEdits);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            if (user is null)
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            await DeleteAsync(user);
        }

        public async Task DeleteBulkAsync(IEnumerable<Guid> ids)
        {
            var users = await GetByIdsAsync(ids.ToList());
            if (!users.Any())
            {
                throw new EntityNotFoundException<ApplicationUser>();
            }

            await DeleteBulkAsync(users);
        }
    }
}