using System;
using CocoriCore;

namespace Comptes
{
    public class PosteViewPageQuery : IPageQuery<PosteViewPage>
    {
        public ID<Poste> Id;
    }

    public class PosteViewPage : PageBase<PosteViewPageQuery>
    {
        public OnInitCall<PosteViewQuery, PosteViewResponse> Poste;
        public PosteUpdatePageQuery Modifier;
    }

    public class PosteViewPageModule : PageModule
    {
        public PosteViewPageModule()
        {
            HandlePage<PosteViewPageQuery, PosteViewPage>((q, p) =>
            {
                p.Poste.Message.Id = q.Id;
                p.Modifier.Id = q.Id;
            })
            .ForAsyncCall(p => p.Poste)
            .MapResponse<PosteViewResponse>()
            .ToSelf();
        }
    }
}