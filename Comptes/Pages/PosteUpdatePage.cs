using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PosteUpdatePageQuery : IPageQuery<PosteUpdatePage>
    {
    }

    public class PosteUpdatePage : PageBase<PosteUpdatePageQuery>
    {
        //public AsyncCall<PosteCreateDefaultValueQuery, PosteCreateDefaultValueResponse> DefaultValue;

        public Form<PosteUpdateCommand, PosteListPageQuery> Enregistrer;

        public PosteUpdatePage()
        {
            //Init(this, x => x.DefaultValue.Result.Nom, x => x.Enregistrer.Command.Nom);
        }
    }

    public class PosteUpdatePageModule : PageModule
    {
        public PosteUpdatePageModule()
        {
            HandlePage<PosteUpdatePageQuery, PosteUpdatePage>()
                //.ForAsyncCall(p => p.DefaultValue)
                //.MapResponse<PosteCreateDefaultValueResponse>()
                //.ToSelf()

                .ForForm(p => p.Enregistrer)
                .MapResponse<Void>()
                .ToModel<PosteListPageQuery>((q, r, m) => { });

        }
    }
}