using LibraryManagementEF.Core.Entities;

public interface IBookRepository
{
    Task<Book?> GetAsync(int id);
    Task<Book?> GetByISBNAsync(string isbn);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book> AddAsync(Book entity);
    Task<Book> UpdateAsync(Book entity);
    Task DeleteAsync(Book entity);
}
