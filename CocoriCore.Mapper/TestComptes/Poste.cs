using System;

namespace CocoriCore.Mapper.Comptes
{
    class Poste : IEntity
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