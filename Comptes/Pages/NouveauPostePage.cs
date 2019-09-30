using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class NouveauPostePageQuery : IPageQuery<NouveauPostePage>
    {
    }

    public class NouveauPostePage : PageBase<NouveauPostePageQuery>
    {
        public AsyncCall<PosteCreateDefaultValueQuery, PosteCreateDefaultValueResponse> DefaultValue;

        public Form<PosteCreateCommand, PageListePostesQuery> Creer;

        public NouveauPostePage()
        {
            Bind(this, x => x.DefaultValue.Result.Nom, x => x.Creer.Command.Nom);
        }
    }

    public class NouveauPostePageModule : PageModule
    {
        public NouveauPostePageModule()
        {
            HandlePage<NouveauPostePageQuery, NouveauPostePage>()
                .ForAsyncCall(p => p.DefaultValue)
                .MapResponse<PosteCreateDefaultValueResponse>()
                .ToSelf()

                .ForForm(p => p.Creer)
                .MapResponse<ID<Poste>>()
                .ToModel<PageListePostesQuery>((q, r, m) => { });

        }
    }
}