using LibraryManagementEF.Core.Entities;

namespace LibraryManagementEF.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task DeleteAsync(User entity);
        Task<IEnumerable<User>> GetUsersInRoleAsync();
        Task<bool> AssignRoleAsync(Guid userId, string roleName);
        Task<bool> RemoveRoleAsync(Guid userId, string roleName);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
    }
}