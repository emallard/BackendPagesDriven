using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class DepenseCreatePageQuery : IPageQuery<DepenseCreatePage>
    {
    }

    public class DepenseCreatePage : PageBase<DepenseCreatePageQuery>
    {
        public AsyncCall<DepenseCreateInitQuery, DepenseCreateInitResponse> Init;
        public Select<PosteListQuery, PosteListResponseItem> PosteSelect;
        public Form<DepenseCreateCommand, ListeDepensesPageQuery> Creer;

        public DepenseCreatePage()
        {
            Init(this, x => x.Init.Result.Poste, x => x.PosteSelect.Selected);
            Input(this, x => x.PosteSelect, x => x.Creer.Command.IdPoste);
            Bind(this, x => x.PosteSelect.Selected.Id, x => x.Creer.Command.IdPoste);
        }
    }

    public class DepenseCreatePageModule : PageModule
    {
        public DepenseCreatePageModule()
        {
            HandlePage<DepenseCreatePageQuery, DepenseCreatePage>((q, p) =>
            {
                p.PosteSelect.Source.Query = new PosteListQuery();
            })
                .ForForm(p => p.Creer)
                .MapResponse<ID<Depense>>()
                .ToModel<ListeDepensesPageQuery>((c, r, m) => { })

                .ForAsyncCall(p => p.PosteSelect.Source)
                .MapResponse<PosteListResponseItem[]>()
                .ToSelf()

                .ForAsyncCall(p => p.Init)
                .MapResponse<DepenseCreateInitResponse>()
                .ToSelf();

        }
    }
}