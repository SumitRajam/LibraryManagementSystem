using System.Security.Claims;
using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementEF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;
        private readonly ILogger<BorrowController> _logger;

        public BorrowController(
            IBorrowService borrowService,
            ILogger<BorrowController> logger)
        {
            _borrowService = borrowService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Librarian, Admin")]
        public async Task<ActionResult<BorrowResponseDTO>> BorrowBook(BorrowRequestDTO requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var borrowRecord = await _borrowService.BorrowBookAsync(requestDto);
                return Ok(borrowRecord);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found during borrowing");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation during borrowing");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error borrowing book");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{borrowId}/return")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<BorrowResponseDTO>> ReturnBook(int borrowId)
        {
            try
            {
                var borrowRecord = await _borrowService.ReturnBookAsync(borrowId);
                return Ok(borrowRecord);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Borrow record not found: {borrowId}");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid return operation: {borrowId}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error returning book for record: {borrowId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BorrowResponseDTO>>> GetUserBorrowedBooks(Guid userId)
        {
            try
            {
                var borrowRecords = await _borrowService.GetUserBorrowedBooksAsync(userId);
                return Ok(borrowRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting borrowed books for user: {userId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<IEnumerable<BorrowResponseDTO>>> GetPendingReturns()
        {
            try
            {
                var pendingReturns = await _borrowService.GetPendingReturnsAsync();
                return Ok(pendingReturns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending returns");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{userId}/pending")]
        public async Task<ActionResult<IEnumerable<BorrowResponseDTO>>> GetUserPendingReturns(Guid userId)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != userId.ToString() && !User.IsInRole("Librarian") && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                var pendingBooks = await _borrowService.GetUserPendingReturnsAsync(userId);
                return Ok(pendingBooks);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User not found: {userId}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting pending books for user: {userId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}