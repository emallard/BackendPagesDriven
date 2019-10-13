using System;
using CocoriCore;

namespace MultiUser
{
    public class UserClaims : IClaims
    {
        public ID<Utilisateur> IdUtilisateur;

        public DateTime ExpireAt => new DateTime(3000, 1, 1);
    }
}