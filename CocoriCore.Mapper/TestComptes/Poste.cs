using System;

namespace CocoriCore.Mapper.Comptes
{
    class Poste : IEntity
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