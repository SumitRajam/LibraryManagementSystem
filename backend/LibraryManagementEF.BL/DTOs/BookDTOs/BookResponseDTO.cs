namespace LibraryManagementEF.BL.DTOs;
public class BookResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int PublishedYear { get; set; }
    public int AvailableCopies { get; set; }
    public int TotalCopies { get; set; } 
}