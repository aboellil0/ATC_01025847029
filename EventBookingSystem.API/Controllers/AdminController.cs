using EventBookingSystem.Core.DTOs.Auth;
using EventBookingSystem.Core.DTOs.Bookings;
using EventBookingSystem.Core.DTOs.User;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminService _adminService;
        private readonly IBookingService _bookingService;
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("dashboard"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<DashboardOverviewModel>> GetDashboardStats()
        {
            try
            {
                var stats = await _adminService.getDashboardStats();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dashboard statistics");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("bookings"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<IReadOnlyList<BookingDto>>> GetAllBookings()
        {
            try
            {
                var bookings = await _adminService.GetAllBookings();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all bookings");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("bookings/{id}")]
        public async Task<ActionResult<BookingDto>> GetBookingById(Guid id)
        {
            try
            {
                var booking = await _adminService.GetBookingById(id);
                if (booking == null)
                {
                    return NotFound();
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching booking by ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("createAdmin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateAdmin([FromBody] RegisterReq registerReq)
        {
            var admin = await _adminService.CreateAdminAsync(registerReq);
            if (admin == null)
            {
                return BadRequest("Failed to create admin");
            }
            return Ok(admin);
        }
    }
}
