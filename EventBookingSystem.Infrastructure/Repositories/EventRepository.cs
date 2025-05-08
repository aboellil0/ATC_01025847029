using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Repositories;
using EventBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories
{
    public class EventRepository :IEventRepository
    {
        private readonly ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Event>> GetEventsByCategoryAsync(EventCategory category)
        {
            return await _context.Events
                .Where(e => e.Category == category)
            .OrderBy(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<bool> IsEventBookedByUserAsync(Guid eventId, Guid userId)
        {
            return await _context.Bookings
                .AnyAsync(b => b.Id == eventId && b.UserId == userId);
        }
    }
}
