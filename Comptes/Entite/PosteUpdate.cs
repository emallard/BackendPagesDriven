using CocoriCore;

namespace Comptes
{
    public class PosteUpdate : IUpdate<Poste>
    {
        public TId<Poste> Id { get; set; }
        public string Nom;
    }
}