namespace CocoriCore.Mapper.Comptes
{
    class DepenseUpdate : IUpdate<Depense>
    {
        public TypedId<Depense> Id { get; set; }
        public TypedId<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}