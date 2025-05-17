using EventBookingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.Events
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EventCategory Category { get; set; }
        public string CategoryName { get; set; }
        public DateTime EventDate { get; set; }
        public string FormattedEventDate { get; set; }
        public string Venue { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBooked { get; set; } = false; // Used to determine if current user has booked this event
    }
}
