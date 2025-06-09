namespace LibraryManagementEF.BL.DTOs;
public class BookCreateDTO
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string ISBN { get; set; } = null!;
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
}
