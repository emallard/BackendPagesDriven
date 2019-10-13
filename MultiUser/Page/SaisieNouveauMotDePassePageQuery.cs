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
        public SaisieNouveauMotDePassePage()
        {
            OnInit(this, p => p.PageQuery.IdToken, p => p.ChangerMotDePasse.Command.IdToken);
        }
    }

    public class SaisieNouveauMotDePassePageModule : PageModule
    {
        public SaisieNouveauMotDePassePageModule()
        {
            HandlePage<SaisieNouveauMotDePassePageQuery, SaisieNouveauMotDePassePage>((query, page) =>
            {
                page.Token.Message.IdToken = query.IdToken;
            });

            ForMessage<NouveauMotDePasseCommand>()
            .WithResponse<Empty>()
            .BuildModel<ConnexionPageQuery>((c, r, m) => { });
        }
    }
}