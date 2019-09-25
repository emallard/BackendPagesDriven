namespace CocoriCore.Mapper.Comptes
{
    class PosteView : IView<Poste>
    {
        public TId<Poste> Id { get; set; }
        public string Nom;


    }
}