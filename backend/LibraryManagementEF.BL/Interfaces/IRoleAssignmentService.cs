using LibraryManagementEF.BL.DTOs;

namespace LibraryManagementEF.BL.Interfaces
{
    public interface IRoleAssignmentService
    {
        Task<RoleAssignmentResponseDTO> AssignRoleAsync(RoleAssignmentRequestDTO request);
        Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync(Guid userId);
    }
}
