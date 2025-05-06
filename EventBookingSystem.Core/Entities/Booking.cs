using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTime BookingDate { get; set; }

        // Navigation properties
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
    }
}
