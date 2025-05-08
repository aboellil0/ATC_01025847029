using EventBookingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.Events
{
    public class EventUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EventCategory Category { get; set; }
        public DateTime EventDate { get; set; }
        public string Venue { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
