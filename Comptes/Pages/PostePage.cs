using System;
using CocoriCore;

namespace Comptes
{
    public class PostePageQuery : IPageQuery<PostePage>
    {
        public ID<Poste> Id;
    }

    public class PostePage : PageBase<PostePageQuery>
    {
        public AsyncCall<PosteViewQuery, PosteViewResponse> Poste;
    }

    public class PostePageModule : PageModule
    {
        public PostePageModule()
        {
            HandlePage<PostePageQuery, PostePage>((q, p) =>
            {
                p.Poste.Query.Id = q.Id;
            })
            .ForAsyncCall(p => p.Poste)
            .MapResponse<PosteViewResponse>()
            .ToSelf();
        }
    }
}