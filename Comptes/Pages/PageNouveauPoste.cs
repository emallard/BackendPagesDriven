using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PageNouveauPosteQuery : IPageQuery<PageNouveauPoste>
    {
    }

    public class PageNouveauPoste : PageBase<PageNouveauPoste>
    {
        public AsyncCall<PageNouveauPosteQuery, PosteCreateDefault> Modele;
        public Form<CreateCommand<PosteCreate>, PageListePostesQuery> Creer;

        public PageNouveauPoste()
        {
            Bind(x => x.Modele.Result.Nom, x => x.Creer.Command.Object.Nom);
        }
    }

    public class PageNouveauPosteModule : PageModule
    {
        public PageNouveauPosteModule()
        {
            Handle<PageNouveauPosteQuery, PageNouveauPoste>(q => new PageNouveauPoste()
            {
                Modele = new AsyncCall<PageNouveauPosteQuery, PosteCreateDefault>()
                {
                    PageQuery = q
                },
                Creer = new Form<CreateCommand<PosteCreate>, PageListePostesQuery>()
            });

            this.Map<PageNouveauPosteQuery, ViewQuery<PosteCreateDefault>>(q => new ViewQuery<PosteCreateDefault>());
            this.Map<ViewQuery<PosteCreateDefault>, PosteCreateDefault, PosteCreateDefault>((q, r) => r);

            this.Map<CreateCommand<PosteCreate>, Guid, PageListePostesQuery>((q, r) =>
                new PageListePostesQuery()
            );
        }
    }
}