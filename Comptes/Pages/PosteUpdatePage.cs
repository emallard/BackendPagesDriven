using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PosteUpdatePageQuery : IPageQuery<PosteUpdatePage>
    {
        public ID<Poste> Id;
    }

    public class PosteUpdatePage : PageBase<PosteUpdatePageQuery>
    {
        public AsyncCall<PosteUpdateInitQuery, PosteUpdateInitResponse> Poste;

        public Form<PosteUpdateCommand, PosteListPageQuery> Enregistrer;

        public PosteUpdatePage()
        {
            OnInit(this, x => x.Poste.Result.Nom, x => x.Enregistrer.Command.Nom);
            OnInit(this, x => x.PageQuery.Id, x => x.Enregistrer.Command.Id);
        }
    }

    public class PosteUpdatePageModule : PageModule
    {
        public PosteUpdatePageModule()
        {
            HandlePage<PosteUpdatePageQuery, PosteUpdatePage>((q, p) =>
            {
                p.Poste.Query.Id = q.Id;
            })
                .ForAsyncCall(p => p.Poste)
                .MapResponse<PosteUpdateInitResponse>()
                .ToSelf()

                .ForForm(p => p.Enregistrer)
                .MapResponse<CocoriCore.Void>()
                .ToModel<PosteListPageQuery>((q, r, m) => { });

        }
    }
}