using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.DTOs.User;
using System.Net.Http.Json;

namespace EventBookingSystem.Webpage.Services
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = "api/admin"; // Adjust the base URL as needed
        }
        public async Task<DashboardOverviewModel> GetDashboardStats()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/dashboard");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<DashboardOverviewModel>();
        }
        public async Task<List<BookingDto>> GetAllBookings()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/bookings");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BookingDto>>();
        }
        public async Task<BookingDto> GetBookingById(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/bookings/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BookingDto>();
        }
        public async Task<UserDto> CreateAdmin(RegisterReq registerReq)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/createAdmin", registerReq);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
    }
}
