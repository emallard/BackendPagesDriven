using System;
using CocoriCore;

namespace Comptes
{
    public class Poste : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new ID<Poste>() { Id = value }; }
        }
        public ID<Poste> Id { get; set; }
        public ID<User> IdUtilisateur;
        public string Nom;
    }
}