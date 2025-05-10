using EventBookingSystem.Core.DTOs.Events;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                Guid? UserId = null;
                if (User.Identity.IsAuthenticated)
                {
                    UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                }

                var events = await _eventService.GetEventsAsync(UserId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(Guid id)
        {
            try
            {
                Guid? UserId = null;
                if (User.Identity.IsAuthenticated)
                {
                    UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                var eventEntity = await _eventService.GetEventByIdAsync(id, UserId);
                return Ok(eventEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdEvent = await _eventService.CreateEventAsync(eventCreateDto);
                return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}"), Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventUpdateDto eventUpdateDto)
        {
            try
            {
                var EventModel = await _eventService.GetEventByIdAsync(id);
                if (id != EventModel.Id || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedEvent = await _eventService.UpdateEventAsync(id, eventUpdateDto);
                if (updatedEvent == null)
                {
                    return NotFound();
                }
                return Ok(updatedEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            try
            {
                var deleted = await _eventService.DeleteEventAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetEventsByCategory(EventCategory category)
        {
            try
            {
                Guid? UserId = null;
                if (User.Identity.IsAuthenticated)
                {
                    UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                var events = await _eventService.GetEventsByCategoryAsync(category, UserId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
