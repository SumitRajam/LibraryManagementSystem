using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Data;
using LibraryManagementEF.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEF.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User?> GetAsync(Guid id) => await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersInRoleAsync()
        {
            return await Task.FromResult(_userManager.Users.ToList());
        }

        public async Task<bool> AssignRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            var addResult = await _userManager.AddToRoleAsync(user, roleName);
            return addResult.Succeeded;
        }

        public async Task<bool> RemoveRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user != null && (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user == null ? Enumerable.Empty<string>() : await _userManager.GetRolesAsync(user);
        }
    }
}