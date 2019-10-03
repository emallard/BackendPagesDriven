using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class PosteListPageQuery : IPageQuery<PosteListPage>
    {
    }

    public class PosteListPage : PageBase<PosteListPageQuery>
    {
        public PosteCreatePageQuery NouveauPoste;
        public AsyncCall<PosteListQuery, PosteListPageItem[]> Postes;
    }

    public class PosteListPageItem
    {
        public PosteViewPageQuery Lien;
        public PosteListResponseItem Poste;
    }

    public class PosteListPageModule : PageModule
    {
        public PosteListPageModule()
        {
            this.HandlePage<PosteListPageQuery, PosteListPage>()
                //.ForAsyncCall(p => p.Postes)
                .ForAsyncCall<PosteListQuery>()
                .MapResponse<PosteListResponseItem[]>()
                .ToModel<PosteListPageItem[]>((q, r) => r.Select(x => new PosteListPageItem()
                {
                    Lien = new PosteViewPageQuery { Id = x.Id },
                    Poste = x
                }).ToArray());
        }
    }
}