using LibraryManagementEF.Core.Entities;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord?> GetAsync(int id);
    Task<IEnumerable<BorrowRecord>> GetByUserAsync(Guid userId);
    Task<IEnumerable<BorrowRecord>> GetPendingReturnsAsync();
    Task<BorrowRecord> AddAsync(BorrowRecord entity);
    Task<BorrowRecord> UpdateAsync(BorrowRecord entity);
    Task<IEnumerable<BorrowRecord>> GetUserPendingReturnsAsync(Guid userId);
}