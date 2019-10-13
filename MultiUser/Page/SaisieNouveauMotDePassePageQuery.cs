using System;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{

    public class SaisieNouveauMotDePassePageQuery : IPageQuery<SaisieNouveauMotDePassePage>
    {
        public ID<TokenMotDePasseOublie> IdToken;
    }

    public class SaisieNouveauMotDePassePage : PageBase<SaisieNouveauMotDePassePageQuery>
    {
        public OnInitCall<TokenMotDePasseOublieQuery, TokenMotDePasseOublieResponse> Token;
        public Form<NouveauMotDePasseCommand, ConnexionPageQuery> ChangerMotDePasse;
    }

    public class SaisieNouveauMotDePassePageModule : PageModule
    {
        public SaisieNouveauMotDePassePageModule()
        {
            HandlePage<SaisieNouveauMotDePassePageQuery, SaisieNouveauMotDePassePage>();
        }
    }
}