using EventBookingSystem.Core.DTOs.Bookings;
using System.Net.Http.Json;

namespace EventBookingSystem.Webpage.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookingDto> CreateBooking(BookingCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/booking/", dto);
            return await response.Content.ReadFromJsonAsync<BookingDto>();
        }

        public async Task<List<BookingDto>> GetUserBookings()
        {
            return await _httpClient.GetFromJsonAsync<List<BookingDto>>("api/bookings");
        }

        public async Task<bool> CancelBooking(Guid bookingId)
        {
            var response = await _httpClient.DeleteAsync($"api/bookings/{bookingId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Checkbooking(Guid eventId)
        {
            var response = await _httpClient.GetAsync($"api/bookings/check/{eventId}");
            return response.IsSuccessStatusCode;
        }
    }
}
