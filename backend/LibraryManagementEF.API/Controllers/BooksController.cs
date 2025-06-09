using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementEF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(
            IBookService bookService,
            ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetBooks()
        {
            try
            {
                var books = await _bookService.GetBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all books");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<ActionResult<BookResponseDTO>> GetBook(int id)
        {
            try
            {
                var book = await _bookService.GetBookAsync(id);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Book not found: {id}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting book with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<ActionResult<BookResponseDTO>> CreateBook(BookCreateDTO requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var book = await _bookService.AddBookAsync(requestDto);
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating book");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<ActionResult<BookResponseDTO>> UpdateBook(BookUpdateDTO requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var book = await _bookService.UpdateBookAsync(requestDto);
                return Ok(book);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Book not found: {requestDto.Id}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating book with ID: {requestDto.Id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Book not found: {id}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting book with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}