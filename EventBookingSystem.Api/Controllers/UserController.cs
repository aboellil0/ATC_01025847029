using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("Users")]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> getAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var userRoles = await _userRepository.GetUserRolesAsync(user.Id);
                userDtos.Add(new UserDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = userRoles.ToList(),
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    IsEmailVerified = user.IsEmailVerified,
                    IsPhoneVerified = user.IsPhoneVerified
                });
            }
            return Ok(userDtos);
        }

        [HttpGet("User/{userId}")]
        public async Task<ActionResult<UserDto>> GetUserDitails(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var userRoles = await _userRepository.GetUserRolesAsync(user.Id);
            return Ok(new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = userRoles.ToList(),
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt,
                IsEmailVerified = user.IsEmailVerified,
                IsPhoneVerified = user.IsPhoneVerified
            });
        }
        [HttpPut("User/{userId}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid userId, [FromBody] UpdateUserReq updateUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            user.Update(updateUserDto.Email, updateUserDto.UserName, updateUserDto.FirstName, updateUserDto.LastName, updateUserDto.DateOfBirth);
            await _userRepository.UpdateUserAsync(user);
            return new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = user.CreatedAt,
                IsEmailVerified = user.IsEmailVerified,
                IsPhoneVerified = user.IsPhoneVerified
            };
        }

        [HttpDelete("User/{userId}")]
        public async Task<ActionResult<bool>> DeleteUser(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            await _userRepository.DeleteUserAsync(user);
            return Ok(true);
        }
    }
}
