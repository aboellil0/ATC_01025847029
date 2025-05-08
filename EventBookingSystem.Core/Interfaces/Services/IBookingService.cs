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
        Task<BookingDto> CreateBookingAsync(int userId, BookingCreateDto bookingCreateDto);
        Task<IReadOnlyList<BookingDto>> GetUserBookingsAsync(int userId);
        Task<bool> HasUserBookedEventAsync(int userId, int eventId);
    }
}
