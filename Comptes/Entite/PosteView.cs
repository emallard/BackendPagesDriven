using CocoriCore;

namespace Comptes
{
    public class PosteView : IView<Poste>
    {
        public ID<Poste> Id { get; set; }
        public string Nom;


    }
}