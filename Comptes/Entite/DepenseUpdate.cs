using CocoriCore;

namespace Comptes
{
    public class DepenseUpdate : IUpdate<Depense>
    {
        public TId<Depense> Id { get; set; }
        public TId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}