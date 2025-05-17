using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.DTOs.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.User
{
    public class DashboardOverviewModel
    {
        public int totalUsers { get; set; }
        public int totalEvents { get; set; }
        public int totalBookings { get; set; }
        public decimal totalRevenue { get; set; }
        public IReadOnlyList<BookingDto> recentBookings { get; set; }
        public IReadOnlyList<EventDto> recentEvents { get; set; }
    }
}
