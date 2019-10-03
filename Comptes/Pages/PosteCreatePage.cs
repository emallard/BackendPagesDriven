using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PosteCreatePageQuery : IPageQuery<PosteCreatePage>
    {
    }

    public class PosteCreatePage : PageBase<PosteCreatePageQuery>
    {
        public AsyncCall<PosteCreateInitQuery, PosteCreateInitResponse> Poste;

        public Form<PosteCreateCommand, PosteListPageQuery> Creer;

        public PosteCreatePage()
        {
            OnInit(this, x => x.Poste.Result.Nom, x => x.Creer.Command.Nom);
        }
    }

    public class PosteCreatePageModule : PageModule
    {
        public PosteCreatePageModule()
        {
            HandlePage<PosteCreatePageQuery, PosteCreatePage>()
                .ForAsyncCall(p => p.Poste)
                .MapResponse<PosteCreateInitResponse>()
                .ToSelf()

                .ForForm(p => p.Creer)
                .MapResponse<ID<Poste>>()
                .ToModel<PosteListPageQuery>((q, r, m) => { });

        }
    }
}