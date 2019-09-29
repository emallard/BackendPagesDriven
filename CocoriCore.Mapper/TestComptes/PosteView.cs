namespace CocoriCore.Mapper.Comptes
{
    class PosteView : IView<Poste>
    {
        public ID<Poste> Id { get; set; }
        public string Nom;


    }
}