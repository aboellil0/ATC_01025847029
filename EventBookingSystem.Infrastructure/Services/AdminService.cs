using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.DTOs.Events;
using EventBookingSystem.Core.DTOs.User;
using EventBookingSystem.Core.Interfaces.Repositories;
using EventBookingSystem.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBookingService _bookingService;
        private readonly IEventService _eventService;
        private readonly IUserRepository userRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepository _eventRepository;
        public AdminService(IUserRepository userRepository, IEventService eventService, IBookingService bookingService, IBookingRepository bookingRepository, IEventRepository eventRepository)
        {
            this.userRepository = userRepository;
            _eventService = eventService;
            _bookingService = bookingService;
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
        }
        public async Task<DashboardOverview> getDashboardStats()
        {
            var totalUsers = await userRepository.GetAllUsersAsync();
            var totalEvents = await _eventRepository.ListAllAsync();
            var totalBookings = await _bookingRepository.GetAllBookingsAsync();
            var totalRevenue = totalBookings.Sum(b => b.Event.Price);
            var recentBookings = totalBookings.OrderByDescending(b => b.BookingDate).Take(5).ToList();
            var recentEvents = totalEvents.OrderByDescending(b => b.EventDate).Take(5).ToList();
            return new DashboardOverview
            {
                totalUsers = totalUsers.Count(),
                totalEvents = totalEvents.Count(),
                totalBookings = totalBookings.Count(),
                totalRevenue = totalRevenue,
                recentBookings = recentBookings.Select(b => new BookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    EventId = b.EventId,
                    BookingDate = b.BookingDate,
                    Event = new simplieEventInformations
                    {
                        name = b.Event.Name,
                        vanue = b.Event.Venue,
                        startDate = b.Event.EventDate
                    },
                    User = new simpleUserInformations
                    {
                        email = b.User.Email,
                        name = b.User.FirstName + " " + b.User.LastName,
                        userName = b.User.UserName
                    }
                }).ToList(),
                recentEvents = recentEvents.Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Venue = e.Venue,
                    EventDate = e.EventDate,
                    Price = e.Price
                }).ToList(),
            };
        }
        public async Task<IReadOnlyList<BookingDto>> GetAllBookings()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            var bookingDtos = new List<BookingDto>();
            foreach (var booking in bookings)
            {
                var user = await userRepository.GetUserByIdAsync(booking.UserId);
                var eventEntity = await _eventRepository.GetEventByIdAsync(booking.EventId);
                var bookingDto = new BookingDto
                {
                    Id = booking.Id,
                    UserId = booking.UserId,
                    EventId = booking.EventId,
                    BookingDate = booking.BookingDate,
                    Event = new simplieEventInformations
                    {
                        name = eventEntity.Name,
                        vanue = eventEntity.Venue,
                        startDate = eventEntity.EventDate
                    },
                    User = new simpleUserInformations
                    {
                        email = user.Email,
                        name = user.FirstName + " " + user.LastName,
                        userName = user.UserName
                    }
                };
                bookingDtos.Add(bookingDto);
            }
            return bookingDtos;
        }

    }

    
    
}
