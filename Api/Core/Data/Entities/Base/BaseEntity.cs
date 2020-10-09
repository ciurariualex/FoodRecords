using System;

namespace Core.Data.Entities.Base
{
    public class BaseEntity<T> : IEntity<T>, IStableEntity, IDeletable
        where T : struct
    {
        public BaseEntity() => CreatedAt = DateTime.UtcNow;

        public T Id { get; set; }
        public DateTime CreatedAt { get; protected set; }
        public Guid CreatedBy { get; set; } = Guid.Empty;
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual void SoftDelete()
        {
            IsDeleted = true;
        }
    }
}