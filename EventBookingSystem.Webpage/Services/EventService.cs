using EventBookingSystem.Core.DTOs.Events;
using System.Net.Http.Json;

namespace EventBookingSystem.Webpage.Services
{
    public class EventService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EventService> _logger;

        public EventService(HttpClient httpClient, ILogger<EventService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<EventDto>> GetEvents()
        {
            return await _httpClient.GetFromJsonAsync<List<EventDto>>($"api/events");

        }

        public async Task<EventDto> GetEvent(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<EventDto>($"api/events/{id}");
        }

        public async Task<EventDto> CreateEvent(CreateEventDto createEventDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/events", createEventDto);
            return await response.Content.ReadFromJsonAsync<EventDto>();
        }

        public async Task<EventDto> UpdateEvent(int id, CreateEventDto updateEventDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", updateEventDto);
            return await response.Content.ReadFromJsonAsync<EventDto>();
        }

        public async Task DeleteEvent(int id)
        {
            await _httpClient.DeleteAsync($"api/events/{id}");
        }
    }
}
