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
        private readonly IMapper _mapper;
        public BookingService(IBookingRepository bookingRepository, IEventRepository eventRepository, IUserRepository userRepository, ILogger<BookingService> logger, IMapper mapper = null)
        {
            _bookingRepository = bookingRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
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
                EventId = eventEntity.Id,
                EventName = eventEntity.Name,
                EventDate = eventEntity.EventDate,
                Venue = eventEntity.Venue,
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
                    EventId = booking.EventId,
                    EventName = booking.Event.Name,
                    EventDate = booking.Event.EventDate,
                    Venue = booking.Event.Venue,
                    BookingDate = booking.BookingDate
                });
            }

            return bookingDtos;
        }

        public async Task<bool> HasUserBookedEventAsync(Guid userId, Guid eventId)
        {
            return await _bookingRepository.HasUserBookedEventAsync(userId, eventId);
        }

    }
}
