using System;
using CocoriCore;

namespace Comptes
{
    public class User : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new ID<User>() { Id = value }; }
        }
        public ID<User> Id { get; set; }
    }
}