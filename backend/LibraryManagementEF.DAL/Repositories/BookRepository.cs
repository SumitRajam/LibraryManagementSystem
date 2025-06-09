using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEF.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> AddAsync(Book entity)
        {
            await _context.Books.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            _context.Books.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Book entity)
        {
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}