using Core.Common.Api.User;
using Core.Data.Entities.Identity;
using Core.Data.Enums;
using Core.Utils.Automapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IApplicationUserService : IBaseService<ApplicationUser, Guid>
    {
        Task<ICollection<TDto>> GetAllAsync<TDto>()
             where TDto : class, IUserViewable;

        Task<TDto> GetByUserNameAsync<TDto>(string userName)
            where TDto : class, IUserViewable;

        Task<ICollection<TDto>> GetByRolesAsync<TDto>(IEnumerable<Role> roles)
             where TDto : class, IUserViewable;

        Task<TDto> GetByIdAsync<TDto>(Guid id)
            where TDto : class, IUserViewable;

        Task<IEnumerable<TDto>> GetByIdsAsync<TDto>(IEnumerable<Guid> ids)
             where TDto : class, IUserViewable;

        Task<TDto> CreateAsync<TDto>(UserAdd dto)
            where TDto : class, IUserViewable;

        Task<IEnumerable<TDto>> CreateBulkAsync<TDto>(ICollection<UserAdd> dtos)
            where TDto : class, IUserViewable;

        Task UpdateAsync(UserEdit dto);

        Task UpdateRoleAsync(Guid id, UserRoleEdit dto);
        Task UpdateBulkAsync(ICollection<UserEdit> dtos);
        Task DeleteAsync(Guid id);
        Task DeleteBulkAsync(IEnumerable<Guid> ids);
    }
}
