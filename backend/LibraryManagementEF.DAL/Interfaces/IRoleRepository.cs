using LibraryManagementEF.Core.Entities;

namespace LibraryManagementEF.DAL.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetAsync(Guid id);
        Task<Role?> GetByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> AddAsync(Role entity);
        Task<Role> UpdateAsync(Role entity);
        Task DeleteAsync(Role entity);
        Task<bool> RoleExistsAsync(string roleName);
    }
}