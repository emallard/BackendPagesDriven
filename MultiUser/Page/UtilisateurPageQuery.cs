using CocoriCore;

namespace MultiUser
{
    public class UtilisateurPageQuery : IPageQuery<UtilisateurPage>
    {

    }

    public class UtilisateurPage : PageBase<UtilisateurPageQuery>
    {
        public MenuConnecteComponent Menu;
        public OnInitCall<ProfileQuery, ProfileResponse> Profile;
    }

    public class UtilisateurPageModule : PageModule
    {
        public UtilisateurPageModule()
        {
            HandlePage<UtilisateurPageQuery, UtilisateurPage>();
        }

    }
}