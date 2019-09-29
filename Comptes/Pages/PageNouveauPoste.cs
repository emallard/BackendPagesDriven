using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PageNouveauPosteQuery : IPageQuery<PageNouveauPoste>
    {
    }

    public class PageNouveauPoste : PageBase<PageNouveauPosteQuery>
    {
        public AsyncCall<ByIdQuery<PosteCreateDefault>, PosteCreateDefault> Modele;
        public Form<PosteCreateCommand, PageListePostesQuery> Creer;

        public PageNouveauPoste()
        {
            Bind(this, x => x.Modele.Result.Nom, x => x.Creer.Command.Nom);
        }
    }

    public class PageNouveauPosteModule : PageModule
    {
        public PageNouveauPosteModule()
        {
            HandlePage<PageNouveauPosteQuery, PageNouveauPoste>()
                .ForAsyncCall(p => p.Modele)
                .MapResponse<PosteCreateDefault>()
                .ToSelf()

                .ForForm(p => p.Creer)
                .MapResponse<ID<Poste>>()
                .ToModel<PageListePostesQuery>((q, r, m) => { });

        }
    }
}