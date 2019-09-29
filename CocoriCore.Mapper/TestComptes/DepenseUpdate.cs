namespace CocoriCore.Mapper.Comptes
{
    class DepenseUpdate : IUpdate<Depense>
    {
        public ID<Depense> Id { get; set; }
        public ID<Poste> IdPoste;
        public string Description;
        public double Montant;
    }
}