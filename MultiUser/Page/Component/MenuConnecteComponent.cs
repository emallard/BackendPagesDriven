using CocoriCore;

namespace MultiUser
{
    public class MenuConnecteComponent
    {
        public bool IsMenu => true;
        public MultiUserAccueilPageQuery Accueil = new MultiUserAccueilPageQuery();
        public ActionCall<DeconnexionCommand, MultiUserAccueilPageQuery> SeDeconnecter = new ActionCall<DeconnexionCommand, MultiUserAccueilPageQuery>()
        {
            Message = new DeconnexionCommand()
        };
    }

    public class MenuConnecteModule : PageModule
    {
        public MenuConnecteModule()
        {
            ForMessage<DeconnexionCommand>()
            .WithResponse<DeconnexionResponse>()
            .BuildModel<MultiUserAccueilPageQuery>((c, r, m) => { });
        }
    }
}