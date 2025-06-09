namespace LibraryManagementEF.BL.DTOs;
public class BorrowResponseDTO
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = null!;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = null!;
}