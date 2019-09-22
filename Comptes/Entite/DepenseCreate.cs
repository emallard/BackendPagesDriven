using CocoriCore;

namespace Comptes
{
    public class DepenseCreate : ICreate<Depense>
    {
        public TypedId<Depense> Id { get; set; }
        public TypedId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}