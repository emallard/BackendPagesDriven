namespace CocoriCore.Mapper.Comptes
{
    class PosteUpdate : IUpdate<Poste>
    {
        public TId<Poste> Id { get; set; }
        public string Nom;
    }
}