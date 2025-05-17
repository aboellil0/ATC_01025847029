using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EventBookingSystem.Webpage.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public UserService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<IReadOnlyList<UserDto>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/User/Users");
            return response ?? new List<UserDto>();
        }

        public async Task<UserDto> GetUserDetailsAsync(Guid userId)
        {
            var response = await _httpClient.GetFromJsonAsync<UserDto>($"api/User/User/{userId}");
            return response ?? new UserDto();
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserReq updateUserDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/User/User/{userId}", updateUserDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>() ?? new UserDto();
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var response = await _httpClient.DeleteAsync($"api/User/User/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
