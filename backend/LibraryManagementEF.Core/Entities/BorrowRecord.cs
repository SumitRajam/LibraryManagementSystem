namespace LibraryManagementEF.Core.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Guid UserId { get; set; } 
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Borrowed"; 
        public Book Book { get; set; } = null!;
        public User User { get; set; } = null!; 
    }
}