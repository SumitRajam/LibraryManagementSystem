// LibraryManagementEF.BLL/DTOs/RoleDTOs.cs
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementEF.BL.DTOs
{
    public class RoleCreateDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class RoleUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class RoleResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}