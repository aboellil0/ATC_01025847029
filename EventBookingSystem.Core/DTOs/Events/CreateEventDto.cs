using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Core.DTOs.Events
{
    public class CreateEventDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
