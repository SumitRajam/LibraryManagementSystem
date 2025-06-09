using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementEF.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userDto)
        {
            try
            {
                var token = await _authService.RegisterAsync(userDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userDto)
        {
            try
            {
                var token = await _authService.LoginAsync(userDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            return Ok(new { message = "You are authenticated!" });
        }
    }
}