namespace CocoriCore.Mapper.Comptes
{
    class PosteView : IView<Poste>
    {
        public TypedId<Poste> Id { get; set; }
        public string Nom;


    }
}