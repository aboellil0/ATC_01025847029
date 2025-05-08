using EventBookingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Services
{
    public interface IEventService
    {
        Task<IReadOnlyList<EventDto>> GetEventsAsync(int? userId = null);
        Task<EventDto> GetEventByIdAsync(int id, int? userId = null);
        Task<IReadOnlyList<EventDto>> GetEventsByCategoryAsync(EventCategory category, int? userId = null);
        Task<EventDto> CreateEventAsync(EventCreateDto eventCreateDto);
        Task<EventDto> UpdateEventAsync(int id, EventUpdateDto eventUpdateDto);
        Task DeleteEventAsync(int id);
    }
}
