namespace CocoriCore.Mapper.Comptes
{
    class DepenseCreate : ICreate<Depense>
    {
        public TId<Depense> Id { get; set; }
        public TId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}