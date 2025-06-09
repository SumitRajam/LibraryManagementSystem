using LibraryManagementEF.BL.DTOs;
using LibraryManagementEF.BL.Interfaces;
using LibraryManagementEF.Core.Entities;
using LibraryManagementEF.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementEF.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserTokenDTO> RegisterAsync(UserRegisterDTO userDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = new User
            {
                Email = userDto.Email,
                UserName = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign "Member" role by default
            await _userManager.AddToRoleAsync(user, "Member");

            return await GenerateJwtToken(user);
        }

        public async Task<UserTokenDTO> LoginAsync(UserLoginDTO userDto)
        {
            var user = await _userRepository.GetByEmailAsync(userDto.Email);
            if (user == null)
                throw new Exception("User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
            if (!result.Succeeded)
                throw new Exception("Invalid credentials");

            return await GenerateJwtToken(user);
        }

        private async Task<UserTokenDTO> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires
            };
        }
    }
}