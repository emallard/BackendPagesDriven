using System;
using CocoriCore;

namespace Comptes
{
    public class UserClaims : IClaims
    {
        public TypedId<Utilisateur> IdUtilisateur;

        public DateTime ExpireAt => new DateTime(3000, 1, 1);
    }

    public class Utilisateur : IEntity
    {
        public Guid Id { get; set; }
    }
}