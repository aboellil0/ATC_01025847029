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
    public class BookingRepository: IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Booking>> GetBookingsByUserIdAsync(Guid userId)
        {
            return await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<bool> HasUserBookedEventAsync(Guid userId, Guid eventId)
        {
            return await _context.Bookings
                .AnyAsync(b => b.UserId == userId && b.EventId == eventId);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
    }
}
