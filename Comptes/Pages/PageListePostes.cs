using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class PageListePostesQuery : IPageQuery<PageListePostes>
    {
    }

    public class PageListePostes
    {
        public PageNouveauPosteQuery NouveauPoste;
        public AsyncCall<PageListePosteItem[]> Postes;
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
            this.MapAsyncCall<PageListePostesQuery, ListQuery<PosteView>, PosteView[], PageListePosteItem[]>(
                q => new ListQuery<PosteView>(),
                (q, r) => r.Select(x => new PageListePosteItem()
                {
                    Lien = new PagePosteQuery { Id = x.Id.Id },
                    Poste = x
                }).ToArray()
                );
            this.Handle<PageListePostesQuery, PageListePostes>(q => new PageListePostes()
            {
                Postes = new AsyncCall<PageListePosteItem[]>(q),
                NouveauPoste = new PageNouveauPosteQuery()
            });
        }
    }
}