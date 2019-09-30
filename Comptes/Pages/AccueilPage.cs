using System;
using CocoriCore;

namespace Comptes
{
    public class AccueilPageQuery : IPageQuery<AccueilPage>
    {
    }

    public class AccueilPage : PageBase<AccueilPageQuery>
    {
        public PosteListPageQuery ListePostes;
        public ListeDepensesPageQuery ListeDepenses;
    }

    public class AccueilPageModule : PageModule
    {
        public AccueilPageModule()
        {
            HandlePage<AccueilPageQuery, AccueilPage>();
        }
    }
}