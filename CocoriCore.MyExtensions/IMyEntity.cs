using System;

namespace CocoriCore
{
    public class EntityBase<T> : IEntity
    {
        Guid IEntity.Id { get => Id.Id; set { Id = new TId<T>() { Id = value }; } }
        public TId<T> Id { get; set; }
    }
}