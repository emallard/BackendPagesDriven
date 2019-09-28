using System;
using CocoriCore;

namespace Comptes
{
    public class PageAccueilQuery : IPageQuery<PageAccueil>
    {
    }

    public class PageAccueil : PageBase<PageAccueilQuery>
    {
        public PageListePostesQuery ListePostes;
        public PageListeDepensesQuery ListeDepenses;
    }

    public class PageAccueilModule : PageModule
    {
        public PageAccueilModule()
        {
            HandlePage<PageAccueilQuery, PageAccueil>();
        }
    }
}