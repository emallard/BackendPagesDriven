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
        public AsyncCall<DepenseCreateInitQuery, DepenseCreateInitResponse> Depense;
        public Select<PosteListQuery, PosteListResponseItem> PosteSelect;
        public Form<DepenseCreateCommand, ListeDepensesPageQuery> Creer;

        public DepenseCreatePage()
        {
            OnInit(this, x => x.Depense.Result.Poste, x => x.PosteSelect.Selected);
            OnSubmit(this, x => x.PosteSelect.Selected.Id, x => x.Creer.Command.IdPoste);

            //Render(this, x => x.PosteSelect, x => x.Creer.Command.IdPoste);
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

                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseCreateInitResponse>()
                .ToSelf();

        }
    }
}