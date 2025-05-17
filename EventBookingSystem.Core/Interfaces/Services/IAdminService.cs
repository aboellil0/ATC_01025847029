using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Interfaces.Services
{
    public interface IAdminService
    {
        Task<DashboardOverviewModel> getDashboardStats();
        Task<IReadOnlyList<BookingDto>> GetAllBookings();
        Task<BookingDto> GetBookingById(Guid id);
        Task<UserDto> CreateAdminAsync(RegisterReq userDto);
    }
}
