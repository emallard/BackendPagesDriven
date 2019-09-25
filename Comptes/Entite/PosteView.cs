using CocoriCore;

namespace Comptes
{
    public class PosteView : IView<Poste>
    {
        public TId<Poste> Id { get; set; }
        public string Nom;


    }
}