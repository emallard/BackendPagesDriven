using System;
using CocoriCore;

namespace Comptes
{

    public class AccueilPageQuery : IPageQuery<AccueilPage>
    {
    }

    public class AccueilPage : PageBase<AccueilPageQuery>
    {
        public MenuComponent Menu;
        public PosteListPageQuery ListePostes;
        public DepenseListPageQuery ListeDepenses;
    }

    public class AccueilPageModule : PageModule
    {
        public AccueilPageModule()
        {
            HandlePage<AccueilPageQuery, AccueilPage>();
        }
    }
}