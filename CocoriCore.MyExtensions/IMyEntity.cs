using System;

namespace CocoriCore
{
    public class EntityBase<T> : IEntity
    {
        Guid IEntity.Id { get => Id.Id; set { Id = new ID<T>() { Id = value }; } }
        public ID<T> Id { get; set; }
    }
}