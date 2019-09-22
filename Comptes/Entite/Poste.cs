using System;
using CocoriCore;

namespace Comptes
{
    public class Poste : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new TypedId<Poste>() { Id = value }; }
        }
        public TypedId<Poste> Id { get; set; }
        public TypedId<User> IdUtilisateur;
        public string Nom;
    }
}