namespace CocoriCore.Mapper.Comptes
{
    class PosteUpdate : IUpdate<Poste>
    {
        public TypedId<Poste> Id { get; set; }
        public string Nom;
    }
}