using EventBookingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<IReadOnlyList<Booking>> GetBookingsByUserIdAsync(Guid userId);
        Task<IReadOnlyList<Booking>> GetBookingsByEventIdAsync(Guid eventId);
        Task<IReadOnlyList<Booking>> GetAllBookingsAsync();
        Task<bool> HasUserBookedEventAsync(Guid userId, Guid eventId);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> UpdateBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(Guid bookingId);
        Task<Booking> GetBookingByIdAsync(Guid bookingId);
    }
}
