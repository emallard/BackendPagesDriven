using System;
using System.Collections.Generic;
using System.Linq;
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
        public Select<PosteListQuery, ValueLabel<ID<Poste>>> PosteSelect;
        public Form<DepenseCreateCommand, DepenseListPageQuery> Creer;

        public DepenseCreatePage()
        {
            OnInit(this, x => x.Depense.Result.Poste, x => x.PosteSelect.Selected);
            OnInit(this, x => x.Depense.Result.Date, x => x.Creer.Command.Date);
            OnSubmit(this, x => x.PosteSelect.Selected.Value, x => x.Creer.Command.IdPoste);
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
                .ToModel<DepenseListPageQuery>((c, r, m) => { })

                .ForAsyncCall(p => p.PosteSelect.Source)
                .MapResponse<PosteListResponseItem[]>()
                .ToSelf()

                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseCreateInitResponse>()
                .ToSelf();

            OnMessage<PosteListQuery>()
                .WithResponse<PosteListResponseItem[]>()
                .BuildModel<ValueLabel<ID<Poste>>[]>((q, r) =>
                {
                    return r.Select(x => new ValueLabel<ID<Poste>>()
                    {
                        Value = x.Id,
                        Label = x.Nom
                    }).ToArray();
                });
        }
    }
}