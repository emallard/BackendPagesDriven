using System;
using System.Linq;
using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{

    public class ConnexionPageQuery : IPageQuery<ConnexionPage>, IQuery
    {

    }

    public class ConnexionPage : PageBase<ConnexionPageQuery>
    {
        public InscriptionPageQuery Inscription;
        public MotDePasseOubliePageQuery MotDePasseOublie;
        public Form<ConnexionCommand, ConnexionPage_SeConnecterReponse> SeConnecter;
    }

    public class ConnexionPage_SeConnecterReponse : IClaimsResponse
    {

        public IClaims Claims;
        public UtilisateurPageQuery UtilisateurPage;

        public IClaims GetClaims()
        {
            return Claims;
        }

        public object GetResponse()
        {
            return UtilisateurPage;
        }
    }


    public class ConnexionPageModule : PageModule
    {
        public ConnexionPageModule()
        {
            HandlePage<ConnexionPageQuery, ConnexionPage>();
            ForMessage<ConnexionCommand>()
            .WithResponse<ConnexionResponse>()
            .BuildModel<ConnexionPage_SeConnecterReponse>((c, r, m) =>
            {
                m.Claims = r.Claims;
                m.UtilisateurPage = new UtilisateurPageQuery();
            });
        }
    }

}