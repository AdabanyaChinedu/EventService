using EventService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventService.Domain.Entities
{
    public class Event : BaseEntity<Guid>, IAggregateRoot
    {
        public Event(string title, string description, string location, DateTime startDate, DateTime endDate, string timeZone, int userId)
        {
            Title = title;
            Description = description;
            Location = location;
            StartDate = startDate;
            EndDate = endDate;
            TimeZone = timeZone;
            UserId = userId;
        }

        public Event(Guid id, string title, string description, string location, DateTime startDate, DateTime endDate, string timeZone, int userId)
            : this(title, description, location, startDate, endDate, timeZone, userId)
        {
            Id = id;
        }

        public Event(Guid id)
        {
            Id = id;
        }

        protected Event()
        {
        }

        public string Title { get; protected set; }

        public string Description { get; protected set; }

        public string Location { get; protected set; }

        public DateTime StartDate { get; protected set; }

        public DateTime EndDate { get; protected set; }

        public string TimeZone { get; protected set; }

        public int UserId { get; protected set; }

    }
}
