using System;
using CocoriCore;

namespace MultiUser
{

    public class MultiUserAccueilPageQuery : IPageQuery<MultiUserAccueilPage>
    {
    }

    public class MultiUserAccueilPage : PageBase<MultiUserAccueilPageQuery>
    {
        public MenuNonConnecteComponent Menu;
    }

    public class MultiUserAccueilPageModule : PageModule
    {
        public MultiUserAccueilPageModule()
        {
            HandlePage<MultiUserAccueilPageQuery, MultiUserAccueilPage>();
        }
    }
}