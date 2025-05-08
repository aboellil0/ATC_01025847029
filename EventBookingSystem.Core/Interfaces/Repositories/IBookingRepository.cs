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
        Task<bool> HasUserBookedEventAsync(Guid userId, Guid eventId);
    }
}
