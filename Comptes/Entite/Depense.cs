using System;
using CocoriCore;

namespace Comptes
{
    public class Depense : IEntity
    {
        Guid IEntity.Id { get => Id.Id; set { Id = new ID<Depense>() { Id = value }; } }
        public ID<Depense> Id { get; set; }
        public ID<User> IdUtilisateur;
        public ID<Poste> IdPoste;
        public string Description;
        public double Montant;
        public DateTime Date;
    }
}