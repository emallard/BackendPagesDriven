using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class PageListePostesQuery : IPageQuery<PageListePostes>
    {
    }

    public class PageListePostes : PageBase<PageListePostesQuery>
    {
        public PageNouveauPosteQuery NouveauPoste;
        public AsyncCall<ListQuery<PosteView>, PageListePosteItem[]> Postes;
    }

    public class PageListePosteItem
    {
        public PagePosteQuery Lien;
        public PosteView Poste;
    }

    public class PageListePostesModule : PageModule
    {
        public PageListePostesModule()
        {
            this.HandlePage<PageListePostesQuery, PageListePostes>()
                .ForAsyncCall(p => p.Postes)
                .MapResponse<PosteView[]>()
                .ToModel<PageListePosteItem[]>((q, r) => r.Select(x => new PageListePosteItem()
                {
                    Lien = new PagePosteQuery { Id = x.Id.Id },
                    Poste = x
                }).ToArray());
        }
    }
}