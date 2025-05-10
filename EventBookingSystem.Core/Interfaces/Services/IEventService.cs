using EventBookingSystem.Core.DTOs.Events;
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
        Task<IReadOnlyList<EventDto>> GetEventsAsync(Guid? userId = null);
        Task<EventDto> GetEventByIdAsync(Guid id, Guid? userId = null);
        Task<IReadOnlyList<EventDto>> GetEventsByCategoryAsync(EventCategory category, Guid? userId = null);
        Task<EventDto> CreateEventAsync(CreateEventDto eventCreateDto);
        Task<EventDto> UpdateEventAsync(Guid id, EventUpdateDto eventUpdateDto);
        Task<bool> DeleteEventAsync(Guid id);
    }
}
