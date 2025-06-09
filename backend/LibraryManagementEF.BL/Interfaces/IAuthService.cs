using LibraryManagementEF.BL.DTOs;

namespace LibraryManagementEF.BL.Interfaces
{
    public interface IAuthService
    {
        Task<UserTokenDTO> RegisterAsync(UserRegisterDTO userDto);
        Task<UserTokenDTO> LoginAsync(UserLoginDTO userDto);
    }
}