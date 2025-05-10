using AutoMapper;
using EventBookingSystem.Core.DTOs.Events;
using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Repositories;
using EventBookingSystem.Core.Interfaces.Services;
using EventBookingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<EventService> _logger;
        public EventService(IEventRepository eventRepository, IMapper mapper, IBookingRepository bookingRepository, ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }
        public async Task<IReadOnlyList<EventDto>> GetEventsAsync(Guid? userId = null)
        {
            var events = await _eventRepository.ListAllAsync();
            var eventDtos = _mapper.Map<IReadOnlyList<EventDto>>(events);

            if (userId != null) {
                foreach (var eventDto in eventDtos)
                {
                    var isBooked = await _bookingRepository.HasUserBookedEventAsync(userId.Value, eventDto.Id);
                    eventDto.IsBooked = isBooked;
                }
            }
            return eventDtos;
        }
        public async Task<EventDto> GetEventByIdAsync(Guid id, Guid? userId = null)
        {
            var eventModel = await _eventRepository.GetEventByIdAsync(id);

            if (eventModel == null)
            {
                _logger.LogError($"Event with id {id} not found");
                throw new Exception($"Event By id {id} not found");
            }

            var eventDto = _mapper.Map<EventDto>(eventModel);
            if (userId != null)
            {
                var isBooked = await _bookingRepository.HasUserBookedEventAsync(userId.Value, eventModel.Id);
                eventDto.IsBooked = isBooked;
            }
            _logger.LogInformation($"Event by id {id} has been found");
            return eventDto;
        }
        public async Task<IReadOnlyList<EventDto>> GetEventsByCategoryAsync(EventCategory category, Guid? userId = null)
        {
            var events = await _eventRepository.GetEventsByCategoryAsync(category);
            var eventDtos = _mapper.Map<IReadOnlyList<EventDto>>(events);
            if (userId != null)
            {
                foreach (var eventDto in eventDtos)
                {
                    var isBooked = await _bookingRepository.HasUserBookedEventAsync(userId.Value, eventDto.Id);
                    eventDto.IsBooked = isBooked;
                }
            }
            _logger.LogInformation($"Events by category {category} has been found");
            return eventDtos;
        }
        public async Task<EventDto> CreateEventAsync(CreateEventDto eventCreateDto)
        {
            var eventModel = new Event()
            {
                Name=eventCreateDto.Name,
                Description = eventCreateDto.Description,
                EventDate = eventCreateDto.EventDate,
                Venue = eventCreateDto.Venue,
                Price = eventCreateDto.Price,
                ImageUrl = eventCreateDto.ImageUrl,
                Category = (EventCategory)eventCreateDto.Category
            };
            var createdEvent = await _eventRepository.CreateEventAsync(eventModel);
            _logger.LogInformation($"event has been created");
            return new EventDto()
            {
                Id=createdEvent.Id,
                Name = createdEvent.Name,
                Description = createdEvent.Description,
                EventDate = createdEvent.EventDate,
                Venue = createdEvent.Venue,
                Price = createdEvent.Price,
                ImageUrl = createdEvent.ImageUrl,
                Category = createdEvent.Category,
                CategoryName = Enum.GetName(typeof(EventCategory), createdEvent.Category),
                FormattedEventDate = createdEvent.EventDate.ToString("dd/MM/yyyy"),
                IsBooked = false // Default value, will be updated when fetching the event
            };   
        }
        public async Task<EventDto> UpdateEventAsync(Guid id, EventUpdateDto eventUpdateDto)
        {
            var eventModel = await _eventRepository.GetEventByIdAsync(id);
            if (eventModel == null)
            {
                _logger.LogError($"Event with id {id} not found");
                throw new Exception($"Event By id {id} not found");
            }

            _mapper.Map(eventModel, eventUpdateDto);
            var updatedEvent = await _eventRepository.UpdateEventAsync(eventModel);

            _logger.LogInformation($"Event with id {id} has been updated");
            return _mapper.Map<EventDto>(updatedEvent);

        }
        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var eventModel = await _eventRepository.GetEventByIdAsync(id);
            if (eventModel == null)
            {
                _logger.LogError($"Event with id {id} not found");
                throw new Exception($"Event By id {id} not found");
            }

            _logger.LogInformation($"Deleted {id}");
            return await _eventRepository.DeleteEventAsync(eventModel);

        }
    }
}
