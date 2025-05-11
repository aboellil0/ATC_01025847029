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
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<EventService> _logger;
        public EventService(IEventRepository eventRepository, IMapper mapper, IBookingRepository bookingRepository, ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }
        public async Task<IReadOnlyList<EventDto>> GetEventsAsync(Guid? userId = null)
        {
            var events = await _eventRepository.ListAllAsync();
            //var eventDtos = _mapper.Map<IReadOnlyList<EventDto>>(events);
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Price = e.Price,
                ImageUrl = e.ImageUrl,
                Category = e.Category,
                CategoryName = Enum.GetName(typeof(EventCategory), e.Category),
                FormattedEventDate = e.EventDate.ToString("dd/MM/yyyy"),
                IsBooked = e.IsBooked
            }).ToList();

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

            //var eventDto = _mapper.Map<EventDto>(eventModel);
            var eventDto = new EventDto()
            {
                Id = id,
                Name = eventModel.Name,
                Description = eventModel.Description,
                EventDate = eventModel.EventDate,
                Venue = eventModel.Venue,
                Price = eventModel.Price,
                ImageUrl = eventModel.ImageUrl,
                Category = eventModel.Category,
                CategoryName = Enum.GetName(typeof(EventCategory), eventModel.Category),
                FormattedEventDate = eventModel.EventDate.ToString("dd/MM/yyyy"),
                IsBooked = eventModel.IsBooked 
            };
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
            //var eventDtos = _mapper.Map<IReadOnlyList<EventDto>>(events);
            var eventDtos = events.Select(e => new EventDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                EventDate = e.EventDate,
                Venue = e.Venue,
                Price = e.Price,
                ImageUrl = e.ImageUrl,
                Category = e.Category,
                CategoryName = Enum.GetName(typeof(EventCategory), e.Category),
                FormattedEventDate = e.EventDate.ToString("dd/MM/yyyy"),
                IsBooked = e.IsBooked
            }).ToList();

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

            //_mapper.Map(eventModel, eventUpdateDto);
            eventModel.Name = eventUpdateDto.Name;
            eventModel.Description = eventUpdateDto.Description;
            eventModel.EventDate = eventUpdateDto.EventDate;
            eventModel.Venue = eventUpdateDto.Venue;
            eventModel.Price = eventUpdateDto.Price;
            eventModel.ImageUrl = eventUpdateDto.ImageUrl;
            eventModel.Category = (EventCategory)eventUpdateDto.Category;

            var updatedEvent = await _eventRepository.UpdateEventAsync(eventModel);

            _logger.LogInformation($"Event with id {id} has been updated");
            //return _mapper.Map<EventDto>(updatedEvent);
            return new EventDto()
            {
                Id = id,
                Name = updatedEvent.Name,
                Description = updatedEvent.Description,
                EventDate = updatedEvent.EventDate,
                Venue = updatedEvent.Venue,
                Price = updatedEvent.Price,
                ImageUrl = updatedEvent.ImageUrl,
                Category = updatedEvent.Category,
                CategoryName = Enum.GetName(typeof(EventCategory), updatedEvent.Category),
                FormattedEventDate = updatedEvent.EventDate.ToString("dd/MM/yyyy"),
                IsBooked = updatedEvent.IsBooked // Default value, will be updated when fetching the event
            };
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
