namespace LibraryManagementEF.BL.DTOs
{
    public class RoleAssignmentRequestDTO
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
    }
}
