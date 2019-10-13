using System;
using CocoriCore;

namespace MultiUser
{
    public class Profile : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new ID<Profile>() { Id = value }; }
        }
        public ID<Profile> Id { get; set; }
        public ID<Utilisateur> IdUtilisateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}