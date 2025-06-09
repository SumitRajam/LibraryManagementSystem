using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementEF.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(
            IRoleRepository roleRepository,
            RoleManager<Role> roleManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
        }

        public async Task<RoleResponseDTO> GetRoleAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id);
            return MapToDto(role);
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(MapToDto);
        }

        public async Task<RoleResponseDTO> CreateRoleAsync(RoleCreateDTO requestDto)
        {
            var role = new Role
            {
                Name = requestDto.Name,
                Description = requestDto.Description
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return MapToDto(role);
        }

        public async Task<RoleResponseDTO> UpdateRoleAsync(RoleUpdateDTO requestDto)
        {
            var role = await _roleRepository.GetAsync(requestDto.Id)
                ?? throw new Exception("Role not found");

            role.Description = requestDto.Description;

            var updatedRole = await _roleRepository.UpdateAsync(role);
            return MapToDto(updatedRole);
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var role = await _roleRepository.GetAsync(id)
                ?? throw new Exception("Role not found");

            await _roleRepository.DeleteAsync(role);
        }

        private static RoleResponseDTO MapToDto(Role role)
        {
            return new RoleResponseDTO
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedDate = role.CreatedDate
            };
        }
    }
}