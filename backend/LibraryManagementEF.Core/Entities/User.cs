using Microsoft.AspNetCore.Identity;

namespace LibraryManagementEF.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}