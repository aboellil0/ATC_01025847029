using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        [HttpGet("bookings")]
        public async Task<ActionResult<BookingDto>> GetUserBookings()
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var bookings = await _bookingService.GetUserBookingsAsync(userId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("booking")]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] BookingCreateDto bookingCreateDto)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var booking = await _bookingService.CreateBookingAsync(userId, bookingCreateDto);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("bookings/{bookingId}")]
        public async Task<ActionResult<bool>> CancelBooking(Guid bookingId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _bookingService.CancelBookingAsync(userId, bookingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("bookings/check/{eventId}")]
        public async Task<ActionResult<bool>> CheckBooking(Guid eventId)
        {
            try
            {
                Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var result = await _bookingService.HasUserBookedEventAsync(userId, eventId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
