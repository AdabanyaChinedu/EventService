using EventService.Domain.Entities;
using EventService.SharedLibrary.Model.ResponseModel;
using System.Linq.Expressions;

namespace EventService.Domain.Interfaces
{
    public interface IEventService
    {
        Task<int> AddEventAsync(Event @event);

        Task<int> UpdateEventAsync(Event @event);

        Task<IEnumerable<Event>> GetEventBy(Expression<Func<Event, bool>> predicate);

        Task<(IEnumerable<Event>, PageParams)> GetPagedEventAsync(int pageIndex, int pageSize);

        Task<IEnumerable<Event>> GetAllEventAsync();

        Task<Event> GetByEventIdAsync(Guid id);

        Task<int> RemoveEventAsync(Event @event);
    }
}
