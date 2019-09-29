using System;
using CocoriCore;

namespace Comptes
{
    public class UserClaims : IClaims
    {
        public ID<User> IdUtilisateur;

        public DateTime ExpireAt => new DateTime(3000, 1, 1);
    }
}