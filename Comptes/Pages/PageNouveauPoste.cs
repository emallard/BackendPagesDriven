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
        public AsyncCall<PosteCreateDefault> Modele;
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
                Modele = new AsyncCall<PosteCreateDefault>(q),
                Creer = new Form<CreateCommand<PosteCreate>, PageListePostesQuery>()
            });

            this.MapAsyncCall<PageNouveauPosteQuery, ByIdQuery<PosteCreateDefault>, PosteCreateDefault, PosteCreateDefault>(
                pageQuery => new ByIdQuery<PosteCreateDefault>(),
                (query, response) => response);

            this.MapForm<CreateCommand<PosteCreate>, Guid, PageListePostesQuery>(
                (q, r) => new PageListePostesQuery()
            );
        }
    }
}