using LibraryManagementEF.BL.DTOs;

namespace LibraryManagementEF.BL.Interfaces
{
    public interface IBookService
    {
        Task<BookResponseDTO> GetBookAsync(int id);
        Task<IEnumerable<BookResponseDTO>> GetBooksAsync();
        Task<BookResponseDTO> AddBookAsync(BookCreateDTO requestDto);
        Task<BookResponseDTO> UpdateBookAsync(BookUpdateDTO requestDto);
        Task DeleteBookAsync(int id);
    }
}