using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Interfaces;

namespace LibraryManagementEF.BL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookResponseDTO> GetBookAsync(int id)
        {
            var book = await _bookRepository.GetAsync(id);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            return MapToDto(book);
        }

        public async Task<IEnumerable<BookResponseDTO>> GetBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(MapToDto);
        }

        public async Task<BookResponseDTO> AddBookAsync(BookCreateDTO requestDto)
        {
            var existingBook = await _bookRepository.GetByISBNAsync(requestDto.ISBN);
            if (existingBook != null)
                throw new InvalidOperationException("Book with this ISBN already exists");

            var book = new Book
            {
                Title = requestDto.Title,
                Author = requestDto.Author,
                ISBN = requestDto.ISBN,
                PublishedYear = requestDto.PublishedYear,
                TotalCopies = requestDto.TotalCopies,
                AvailableCopies = requestDto.TotalCopies
            };

            var createdBook = await _bookRepository.AddAsync(book);
            return MapToDto(createdBook);
        }

        public async Task<BookResponseDTO> UpdateBookAsync(BookUpdateDTO requestDto)
        {
            var book = await _bookRepository.GetAsync(requestDto.Id);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            book.Title = requestDto.Title;
            book.Author = requestDto.Author;
            book.PublishedYear = requestDto.PublishedYear;

            if (requestDto.TotalCopies != book.TotalCopies)
            {
                var borrowedCopies = book.TotalCopies - book.AvailableCopies;
                book.TotalCopies = requestDto.TotalCopies;
                book.AvailableCopies = Math.Max(0, book.TotalCopies - borrowedCopies);
            }

            var updatedBook = await _bookRepository.UpdateAsync(book);
            return MapToDto(updatedBook);
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetAsync(id);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            await _bookRepository.DeleteAsync(book);
        }

        private static BookResponseDTO MapToDto(Book book)
        {
            return new BookResponseDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies
            };
        }
    }
}