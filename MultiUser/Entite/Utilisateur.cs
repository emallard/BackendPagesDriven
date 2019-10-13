using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{

    public class Utilisateur : IEntity
    {
        Guid IEntity.Id
        {
            get => Id.Id;
            set { Id = new ID<Utilisateur>() { Id = value }; }
        }
        public ID<Utilisateur> Id { get; set; }
        public string Email { get; set; }
        public string HashMotDePasse { get; set; }


    }
}