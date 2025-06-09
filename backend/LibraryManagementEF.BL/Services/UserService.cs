using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LibraryManagementEF.BLL.Services
{
    public class UserService : IUserService, IRoleAssignmentService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            UserManager<User> userManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetMembersAsync()
        {
            try
            {
                var memberUsers = await _userRepository.GetUsersInRoleAsync();
                return memberUsers?.Select(user => new UserResponseDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName
                }) ?? Enumerable.Empty<UserResponseDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving members");
                throw;
            }
        }

        public async Task<RoleAssignmentResponseDTO> AssignRoleAsync(RoleAssignmentRequestDTO request)
        {
            try
            {
                if (!await _roleRepository.RoleExistsAsync(request.RoleName))
                {
                    _logger.LogWarning($"Role {request.RoleName} not found");
                    return new RoleAssignmentResponseDTO
                    {
                        Success = false,
                        Message = $"Role {request.RoleName} does not exist"
                    };
                }

                var result = await _userRepository.AssignRoleAsync(request.UserId, request.RoleName);

                return new RoleAssignmentResponseDTO
                {
                    Success = result,
                    Message = result ? "Role assigned successfully" : "Failed to assign role"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning role {request.RoleName} to user {request.UserId}");
                throw;
            }
        }

        public async Task<IEnumerable<UserRoleDTO>> GetUserRolesAsync(Guid userId)
        {
            var roles = await _userRepository.GetUserRolesAsync(userId);
            return roles.Select(role => new UserRoleDTO
            {
                RoleName = role,
                IsAssigned = true
            });
        }
    }
}