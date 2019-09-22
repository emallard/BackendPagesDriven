using System;

namespace CocoriCore.Mapper.Comptes
{
    class Depense : IEntity
    {
        Guid IEntity.Id { get => Id.Id; set { Id = new TypedId<Depense>() { Id = value }; } }
        public TypedId<Depense> Id { get; set; }
        public TypedId<User> IdUtilisateur;
        public TypedId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}