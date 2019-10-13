using System;
using CocoriCore;
using CocoriCore.Router;

namespace MultiUser
{

    public class MultiUserRouterConfiguration
    {

        public static void Load(RouterOptionsBuilder builder)
        {
            builder.Get<MultiUserAccueilPageQuery>().SetPath("api/multiuser");
            builder.Get<ConnexionPageQuery>().SetPath("api/connexion");
            builder.Get<InscriptionPageQuery>().SetPath("api/inscription");
            builder.Get<MotDePasseOubliePageQuery>().SetPath("api/users/mot-de-passe-oublie");
            builder.Get<MotDePasseOublieConfirmationPageQuery>().SetPath("api/users/mot-de-passe-oublie/confirmation");
            builder.Get<SaisieNouveauMotDePassePageQuery>().UseQuery().SetPath("api/users/saisie-nouveau-mot-de-passe");
        }
    }
}