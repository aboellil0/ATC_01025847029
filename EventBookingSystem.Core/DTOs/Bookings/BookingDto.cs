using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.Bookings
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public simpleUserInformations User { get; set; }
        public Guid EventId { get; set; }
        public simplieEventInformations Event { get; set; }
        public DateTime BookingDate { get; set; }
    }
    public class simpleUserInformations
    {
        public string name { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
    }
    public class simplieEventInformations
    {
        public string name { get; set; }
        public string vanue { get; set; }
        public DateTime startDate { get; set; }
    }
}