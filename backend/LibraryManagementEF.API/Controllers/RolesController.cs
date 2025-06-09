using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementEF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Librarian")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            IRoleService roleService, ILogger<RolesController> logger, IUserService userService)
        {
            _roleService = roleService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<RoleResponseDTO>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all roles");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleResponseDTO>> GetRoleById([Required] Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleAsync(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting role with ID: {id}");
                return NotFound($"Role with ID {id} not found");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleResponseDTO>> CreateRole([FromBody] RoleCreateDTO requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdRole = await _roleService.CreateRoleAsync(requestDto);
                return CreatedAtAction(
                    nameof(GetRoleById),
                    new { id = createdRole.Id },
                    createdRole);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating role");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleResponseDTO>> UpdateRole([FromBody] RoleUpdateDTO requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedRole = await _roleService.UpdateRoleAsync(requestDto);
                return Ok(updatedRole);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role not found: {requestDto.Id}");
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating role");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating role with ID: {requestDto.Id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteRole([Required] Guid id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Role not found: {id}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting role with ID: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("ListMembers")]
        [Authorize(Roles = "Admin, Librarian")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> ListMembers()
        {
            try
            {
                var members = await _userService.GetMembersAsync();

                if (members == null || !members.Any())
                {
                    _logger.LogInformation("No members found");
                    return NoContent();
                }

                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving members");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}