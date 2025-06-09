using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Data;
using LibraryManagementEF.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEF.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetAsync(Guid id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> AddAsync(Role entity)
        {
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Role> UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _context.Roles.AnyAsync(r => r.Name == roleName);
        }
    }
}