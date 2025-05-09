using EventBookingSystem.Core.DTOs.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(Guid userId, BookingCreateDto bookingCreateDto);
        Task<IReadOnlyList<BookingDto>> GetUserBookingsAsync(Guid userId);
        Task<bool> HasUserBookedEventAsync(Guid userId, Guid eventId);
    }
}
