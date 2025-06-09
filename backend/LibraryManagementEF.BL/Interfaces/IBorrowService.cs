using LibraryManagementEF.BL.DTOs;

namespace LibraryManagementEF.BL.Interfaces;
public interface IBorrowService
{
    Task<BorrowResponseDTO> BorrowBookAsync(BorrowRequestDTO requestDto);
    Task<BorrowResponseDTO> ReturnBookAsync(int borrowId);
    Task<IEnumerable<BorrowResponseDTO>> GetUserBorrowedBooksAsync(Guid userId);
    Task<IEnumerable<BorrowResponseDTO>> GetPendingReturnsAsync();
    Task<IEnumerable<BorrowResponseDTO>> GetUserPendingReturnsAsync(Guid userId);
}