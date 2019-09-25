using System;
using CocoriCore;

namespace Comptes
{
    public class User : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new TId<User>() { Id = value }; }
        }
        public TId<User> Id { get; set; }
    }
}