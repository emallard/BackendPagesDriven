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
        public AsyncCall<PosteViewQuery, PosteViewResponse> Poste;
    }

    public class PosteViewPageModule : PageModule
    {
        public PosteViewPageModule()
        {
            HandlePage<PosteViewPageQuery, PosteViewPage>((q, p) =>
            {
                p.Poste.Query.Id = q.Id;
            })
            .ForAsyncCall(p => p.Poste)
            .MapResponse<PosteViewResponse>()
            .ToSelf();
        }
    }
}