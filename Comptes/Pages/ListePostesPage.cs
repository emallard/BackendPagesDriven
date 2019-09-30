using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class PageListePostesQuery : IPageQuery<ListePostesPage>
    {
    }

    public class ListePostesPage : PageBase<PageListePostesQuery>
    {
        public NouveauPostePageQuery NouveauPoste;
        public AsyncCall<PosteListQuery, ListePostePageItem[]> Postes;
    }

    public class ListePostePageItem
    {
        public PostePageQuery Lien;
        public PosteListResponseItem Poste;
    }

    public class ListePostesPageModule : PageModule
    {
        public ListePostesPageModule()
        {
            this.HandlePage<PageListePostesQuery, ListePostesPage>()
                .ForAsyncCall(p => p.Postes)
                .MapResponse<PosteListResponseItem[]>()
                .ToModel<ListePostePageItem[]>((q, r) => r.Select(x => new ListePostePageItem()
                {
                    Lien = new PostePageQuery { Id = x.Id },
                    Poste = x
                }).ToArray());
        }
    }
}