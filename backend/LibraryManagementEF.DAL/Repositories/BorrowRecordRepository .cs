using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEF.DAL.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BorrowRecord?> GetAsync(int id)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<IEnumerable<BorrowRecord>> GetByUserAsync(Guid userId)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Where(br => br.UserId == userId)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowRecord>> GetPendingReturnsAsync()
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.ReturnDate == null)
                .OrderBy(br => br.DueDate)
                .ToListAsync();
        }

        public async Task<BorrowRecord> AddAsync(BorrowRecord entity)
        {
            await _context.BorrowRecords.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BorrowRecord> UpdateAsync(BorrowRecord entity)
        {
            _context.BorrowRecords.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<BorrowRecord>> GetUserPendingReturnsAsync(Guid userId)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.User) 
                .Where(br => br.UserId == userId && br.ReturnDate == null)
                .OrderBy(br => br.DueDate)
                .ToListAsync();
        }
    }
}