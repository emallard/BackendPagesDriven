using System;
using CocoriCore;

namespace MultiUser
{
    public class TokenMotDePasseOublie : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new ID<TokenMotDePasseOublie>() { Id = value }; }
        }

        public ID<TokenMotDePasseOublie> Id { get; set; }
        public string Email { get; set; }
        public bool Utilise { get; set; }
        public DateTime DateExpiration { get; set; }
    }
}