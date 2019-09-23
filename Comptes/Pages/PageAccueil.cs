using System;
using CocoriCore;

namespace Comptes
{
    public class PageAccueilQuery : IPageQuery<PageAccueil>
    {
    }

    public class PageAccueil
    {
        public PageListePostesQuery ListePostes;
    }

    public class PageAccueilModule : PageModule
    {
        public PageAccueilModule()
        {
            Handle<PageAccueilQuery, PageAccueil>(q => new PageAccueil()
            {
                ListePostes = new PageListePostesQuery()
            });
        }
    }
}