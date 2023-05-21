using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventService.Domain.Entities
{
    public abstract class BaseEntity<TKey>
    {
        protected BaseEntity()
        {
            Id = default(TKey);
        }
        protected BaseEntity(TKey id)
        {
            Id = id;
        }
        public virtual TKey Id { get; protected set; }
        public virtual DateTime CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; set; }
    }
}
