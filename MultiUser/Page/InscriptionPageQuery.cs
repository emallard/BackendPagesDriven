using System.Threading.Tasks;
using CocoriCore;

namespace MultiUser
{
    public class InscriptionPageQuery : IPageQuery<InscriptionPage>, IQuery
    {
    }

    public class InscriptionPage : PageBase<InscriptionPageQuery>
    {
        public ConnexionPageQuery Connexion;
        public Form<InscriptionCommand, InscriptionPage_SInscrireResponse> SInscrire;
    }

    public class InscriptionPage_SInscrireResponse : IClaimsResponse
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


    public class InscriptionPageModule : PageModule
    {
        public InscriptionPageModule()
        {
            HandlePage<InscriptionPageQuery, InscriptionPage>();

            ForMessage<InscriptionCommand>()
            .WithResponse<InscriptionResponse>()
            .BuildModel<InscriptionPage_SInscrireResponse>((c, r, m) =>
            {
                m.Claims = r.Claims;
                m.UtilisateurPage = new UtilisateurPageQuery();
            });
        }
    }
}