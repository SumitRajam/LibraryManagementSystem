using Microsoft.AspNetCore.Identity;

namespace LibraryManagementEF.Core.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}