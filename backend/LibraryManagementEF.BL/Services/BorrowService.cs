using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using LibraryManagementEF.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementEF.BL.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRecordRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;
        private readonly UserManager<User> _userManager;

        public BorrowService(
            IBorrowRecordRepository borrowRepository,
            IBookRepository bookRepository,
            UserManager<User> userManager)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
            _userManager = userManager;
        }

        public async Task<BorrowResponseDTO> BorrowBookAsync(BorrowRequestDTO requestDto)
        {
            var book = await _bookRepository.GetAsync(requestDto.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            if (book.AvailableCopies < 1)
                throw new InvalidOperationException("No available copies of this book");

            var user = await _userManager.FindByIdAsync(requestDto.UserId.ToString());
            if (user == null)
                throw new KeyNotFoundException("User not found");

            book.AvailableCopies--;
            await _bookRepository.UpdateAsync(book);

            var borrowRecord = new BorrowRecord
            {
                BookId = requestDto.BookId,
                UserId = requestDto.UserId,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(requestDto.DaysToBorrow),
                Status = "Borrowed"
            };

            var result = await _borrowRepository.AddAsync(borrowRecord);
            return MapToDto(result, book, user);
        }

        public async Task<BorrowResponseDTO> ReturnBookAsync(int borrowId)
        {
            var borrowRecord = await _borrowRepository.GetAsync(borrowId);
            if (borrowRecord == null)
                throw new KeyNotFoundException("Borrow record not found");

            if (borrowRecord.ReturnDate != null)
                throw new InvalidOperationException("Book already returned");

            var book = await _bookRepository.GetAsync(borrowRecord.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            book.AvailableCopies++;
            await _bookRepository.UpdateAsync(book);

            borrowRecord.ReturnDate = DateTime.UtcNow;
            borrowRecord.Status = "Returned";

            var updatedRecord = await _borrowRepository.UpdateAsync(borrowRecord);
            var user = await _userManager.FindByIdAsync(borrowRecord.UserId.ToString());

            return MapToDto(updatedRecord, book, user!);
        }

        public async Task<IEnumerable<BorrowResponseDTO>> GetUserBorrowedBooksAsync(Guid userId)
        {
            var records = await _borrowRepository.GetByUserAsync(userId);
            return records.Select(r => MapToDto(r, r.Book, r.User));
        }

        public async Task<IEnumerable<BorrowResponseDTO>> GetPendingReturnsAsync()
        {
            var records = await _borrowRepository.GetPendingReturnsAsync();
            return records.Select(r => MapToDto(r, r.Book, r.User));
        }

        private static BorrowResponseDTO MapToDto(BorrowRecord record, Book book, User user)
        {
            return new BorrowResponseDTO
            {
                Id = record.Id,
                BookId = book.Id,
                BookTitle = book.Title,
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                BorrowDate = record.BorrowDate,
                DueDate = record.DueDate,
                ReturnDate = record.ReturnDate,
                Status = record.Status
            };
        }

        public async Task<IEnumerable<BorrowResponseDTO>> GetUserPendingReturnsAsync(Guid userId)
        { 
            var records = await _borrowRepository.GetUserPendingReturnsAsync(userId);
            var user = await _userManager.FindByIdAsync(userId.ToString());
    
            if (user == null)
                throw new KeyNotFoundException("User not found");

            return records.Select(r => MapToDto(r, r.Book, user));
        }
    }
}