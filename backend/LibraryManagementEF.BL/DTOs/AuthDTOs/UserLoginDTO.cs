﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementEF.BL.DTOs
{
    public class UserLoginDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
