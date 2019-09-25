using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PageNouveauPosteQuery : IPageQuery<PageNouveauPoste>
    {
    }

    public class PageNouveauPoste : PageBase<PageNouveauPosteQuery, PageNouveauPoste>
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
            HandlePage<PageNouveauPosteQuery, PageNouveauPoste>();

            On<PageNouveauPosteQuery>()
                .ProvideQuery<ByIdQuery<PosteCreateDefault>>((p, q) => { })
                .WithResponse<PosteCreateDefault>()
                .AsModel();

            this.MapForm<CreateCommand<PosteCreate>, Guid, PageListePostesQuery>(
                (q, r) => new PageListePostesQuery()
            );
        }
    }
}