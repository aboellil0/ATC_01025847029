using AutoMapper;
using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Repositories;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Services
{
    public class BookingService: IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BookingService> _logger;
        public BookingService(IBookingRepository bookingRepository, IEventRepository eventRepository, IUserRepository userRepository, ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BookingDto> CreateBookingAsync(Guid userId, BookingCreateDto bookingCreateDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"User with ID {userId} not found.");
                throw new Exception($"User with ID {userId} not found.");
            }

            var eventEntity = await _eventRepository.GetEventByIdAsync(bookingCreateDto.EventId);
            if (eventEntity == null)
            {
                _logger.LogError($"Event with ID {bookingCreateDto.EventId} not found.");
                throw new Exception($"Event with ID {bookingCreateDto.EventId} not found.");
            }

            var isBooked = await _eventRepository.IsEventBookedByUserAsync(bookingCreateDto.EventId, userId);
            if (isBooked)
            {
                _logger.LogError($"User with ID {userId} has already booked event with ID {bookingCreateDto.EventId}.");
                throw new Exception($"User with ID {userId} has already booked event with ID {bookingCreateDto.EventId}.");
            }

            var booking = new Booking
            {
                UserId = userId,
                EventId = bookingCreateDto.EventId,
                BookingDate = DateTime.UtcNow
            };

            _logger.LogInformation("Booking has been done");
            var createdBooking =  await _bookingRepository.CreateBookingAsync(booking);

            var bookingDto = new BookingDto
            {
                Id = createdBooking.Id,
                UserId = userId,
                User = new simpleUserInformations { email = user.Email, name = user.FirstName+" "+user.LastName, userName = user.UserName },
                EventId = eventEntity.Id,
                Event = new simplieEventInformations { name = eventEntity.Name, vanue = eventEntity.Venue, startDate = eventEntity.EventDate },
                BookingDate = createdBooking.BookingDate
            };

            return bookingDto;
        }

        public async Task<IReadOnlyList<BookingDto>> GetUserBookingsAsync(Guid userId)
        {
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
            if (bookings == null)
            {
                _logger.LogError($"No bookings found for user with ID {userId}.");
                throw new Exception($"No bookings found for user with ID {userId}.");
            }
                var bookingDtos = new List<BookingDto>();

            foreach (var booking in bookings)
            {
                bookingDtos.Add(new BookingDto
                {
                    Id = booking.Id,
                    UserId = booking.UserId,
                    User = new simpleUserInformations { email = booking.User.Email, name = booking.User.FirstName + " " + booking.User.LastName, userName = booking.User.UserName },
                    EventId = booking.Id,
                    Event = new simplieEventInformations { name = booking.Event.Name, vanue = booking.Event.Venue, startDate = booking.Event.EventDate },
                    BookingDate = booking.BookingDate
                });
            }

            return bookingDtos;
        }

        public async Task<bool> HasUserBookedEventAsync(Guid userId, Guid eventId)
        {
            return await _bookingRepository.HasUserBookedEventAsync(userId, eventId);
        }

        public async Task<bool> CancelBookingAsync(Guid userId, Guid bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId)
            {
                _logger.LogError($"Booking with ID {bookingId} not found or does not belong to user with ID {userId}.");
                throw new Exception($"Booking with ID {bookingId} not found or does not belong to user with ID {userId}.");
            }
            await _bookingRepository.DeleteBookingAsync(bookingId);
            _logger.LogInformation($"Booking with ID {bookingId} has been cancelled for user with ID {userId}.");
            return true;
        }
    }
}
