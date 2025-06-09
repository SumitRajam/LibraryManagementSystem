using LibraryManagementEF.BL.DTOs;

namespace LibraryManagementEF.BL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetMembersAsync();
    }
}