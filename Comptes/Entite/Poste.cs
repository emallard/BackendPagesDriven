using System;
using CocoriCore;

namespace Comptes
{
    public class Poste : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new TId<Poste>() { Id = value }; }
        }
        public TId<Poste> Id { get; set; }
        public TId<User> IdUtilisateur;
        public string Nom;
    }
}