using CocoriCore;

namespace Comptes
{
    public class PosteView : IView<Poste>
    {
        public TypedId<Poste> Id { get; set; }
        public string Nom;


    }
}