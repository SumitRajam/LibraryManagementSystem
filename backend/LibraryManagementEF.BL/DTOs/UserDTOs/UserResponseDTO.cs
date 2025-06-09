using System;
using System.Collections.Generic;
using System.Linq;
namespace LibraryManagementEF.BL.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }
}
