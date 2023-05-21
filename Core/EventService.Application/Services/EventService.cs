using EventService.Domain.Entities;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Model.ResponseModel;
using System.Linq.Expressions;

namespace EventService.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _repository;
        public EventService(IRepository<Event> repository)
        {
            _repository = repository;
        }
        public async Task<int> AddEventAsync(Event @event)
        {
            return await _repository.AddAsync(@event);
        }

        public async Task<IEnumerable<Event>> GetAllEventAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Event> GetByEventIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Event>> GetEventBy(Expression<Func<Event, bool>> predicate)
        {
            return await _repository.GetBy(predicate);
        }

        public async Task<(IEnumerable<Event>, PageParams)> GetPagedEventAsync(int pageIndex, int pageSize)
        {
            return await _repository.GetPagedAsync(pageIndex, pageSize);
        }

        public async Task<int> UpdateEventAsync(Event @event)
        {
           return await _repository.UpdateAsync(@event);
        }

        public async Task<int> RemoveEventAsync(Event @event)
        {
            return await _repository.RemoveAsync(@event);
        }
    }
}
