using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Venue { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsBooked { get; set; }
        public EventCategory Category { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

    public enum EventCategory
    {
        Conference = 0,
        Workshop = 1,
        Seminar = 2,
        Concert = 3,
        Exhibition = 4,
        Sports = 5,
        Other = 6
    }
}
