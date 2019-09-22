namespace CocoriCore.Mapper.Comptes
{
    class DepenseView : IView<Depense>, IWith<Poste>
    {
        public TypedId<Depense> Id { get; set; }
        public TypedId<Poste> IdPoste;
        public string NomPoste;
        public string Description;
        public double Montant;
    }
}