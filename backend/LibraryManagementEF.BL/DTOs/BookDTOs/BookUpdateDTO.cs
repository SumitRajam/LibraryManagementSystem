using System.ComponentModel.DataAnnotations;

namespace LibraryManagementEF.BL.DTOs;
public class BookUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Author { get; set; } = null!;

    [Required]
    public int PublishedYear { get; set; }

    [Required]
    public int TotalCopies { get; set; }
}