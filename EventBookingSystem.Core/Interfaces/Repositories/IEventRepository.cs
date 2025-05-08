using EventBookingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<IReadOnlyList<Event>> GetEventsByCategoryAsync(EventCategory category);
        Task<bool> IsEventBookedByUserAsync(Guid eventId, Guid userId);
    }

}
