namespace CocoriCore.Mapper.Comptes
{
    class DepenseView : IJoin<Depense, Poste>
    {
        public TId<Depense> Id { get; set; }
        public TId<Poste> IdPoste;
        public string NomPoste;
        public string Description;
        public double Montant;
    }
}