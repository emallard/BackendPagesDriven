using CocoriCore;

namespace Comptes
{
    public class PosteUpdate : IUpdate<Poste>
    {
        public TypedId<Poste> Id { get; set; }
        public string Nom;
    }
}