namespace CocoriCore.Mapper.Comptes
{
    class PosteUpdate : IUpdate<Poste>
    {
        public ID<Poste> Id { get; set; }
        public string Nom;
    }
}