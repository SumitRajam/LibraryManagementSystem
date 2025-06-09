namespace LibraryManagementEF.BL.DTOs;
public class BorrowRequestDTO
{
    public int BookId { get; set; }
    public Guid UserId { get; set; }
    public int DaysToBorrow { get; set; } = 14;
}
