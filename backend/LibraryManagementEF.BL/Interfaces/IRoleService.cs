using LibraryManagementEF.BL.DTOs;

namespace LibraryManagementEF.BL.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDTO> GetRoleAsync(Guid id);
        Task<IEnumerable<RoleResponseDTO>> GetRolesAsync();
        Task<RoleResponseDTO> CreateRoleAsync(RoleCreateDTO requestDto);
        Task<RoleResponseDTO> UpdateRoleAsync(RoleUpdateDTO requestDto);
        Task DeleteRoleAsync(Guid id);
    }
}