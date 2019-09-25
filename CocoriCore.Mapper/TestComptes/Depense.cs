using System;

namespace CocoriCore.Mapper.Comptes
{
    class Depense : IEntity
    {
        Guid IEntity.Id { get => Id.Id; set { Id = new TId<Depense>() { Id = value }; } }
        public TId<Depense> Id { get; set; }
        public TId<User> IdUtilisateur;
        public TId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}