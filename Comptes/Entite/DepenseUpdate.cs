using CocoriCore;

namespace Comptes
{
    public class DepenseUpdate : IUpdate<Depense>
    {
        public TypedId<Depense> Id { get; set; }
        public TypedId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}