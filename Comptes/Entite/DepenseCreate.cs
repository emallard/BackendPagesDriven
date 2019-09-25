using CocoriCore;

namespace Comptes
{
    public class DepenseCreate : ICreate<Depense>
    {
        public TId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}