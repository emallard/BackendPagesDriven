using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class PageListePostesQuery : IMessage<PageListePostes>
    {
    }

    public class PageListePostes
    {
        public PageNouveauPosteQuery NouveauPoste;
        public AsyncCall<PageListePostesQuery, PageListePosteItem[]> Postes;
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
            this.Map<PageListePostesQuery, ListQuery<PosteView>>(q => new ListQuery<PosteView>());
            this.Map<ListQuery<PosteView>, PosteView[], PageListePosteItem[]>((q, r) => r.Select(x => new PageListePosteItem()
            {
                Lien = new PagePosteQuery { Id = x.Id.Id },
                Poste = x
            }).ToArray());
            this.Handle<PageListePostesQuery, PageListePostes>(q => new PageListePostes()
            {
                Postes = new AsyncCall<PageListePostesQuery, PageListePosteItem[]>(),
                NouveauPoste = new PageNouveauPosteQuery()
            });
        }
    }
}