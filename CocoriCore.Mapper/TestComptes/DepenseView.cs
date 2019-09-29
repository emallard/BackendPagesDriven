namespace CocoriCore.Mapper.Comptes
{
    class DepenseView : IJoin<Depense, Poste>
    {
        public ID<Depense> Id { get; set; }
        public ID<Poste> IdPoste;
        public string NomPoste;
        public string Description;
        public double Montant;
    }
}