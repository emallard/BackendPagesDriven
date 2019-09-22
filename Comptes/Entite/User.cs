using System;
using CocoriCore;

namespace Comptes
{
    public class User : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new TypedId<User>() { Id = value }; }
        }
        public TypedId<User> Id { get; set; }
    }
}