using LibraryManagementEF.BL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementEF.BL.Interfaces;

namespace LibraryManagementEF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleAssignmentController : ControllerBase
    {
        private readonly IRoleAssignmentService _roleAssignmentService;
        private readonly ILogger<RoleAssignmentController> _logger;

        public RoleAssignmentController(
            IRoleAssignmentService roleAssignmentService,
            ILogger<RoleAssignmentController> logger)
        {
            _roleAssignmentService = roleAssignmentService;
            _logger = logger;
        }

        [HttpPost("assign")]
        [ProducesResponseType(typeof(RoleAssignmentResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RoleAssignmentResponseDTO>> AssignRole([FromBody] RoleAssignmentRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _roleAssignmentService.AssignRoleAsync(request);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user-roles/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<UserRoleDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserRoleDTO>>> GetUserRoles(Guid userId)
        {
            try
            {
                return Ok(await _roleAssignmentService.GetUserRolesAsync(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting roles for user {userId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}