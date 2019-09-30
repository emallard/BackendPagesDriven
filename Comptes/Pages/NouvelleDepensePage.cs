using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class NouvelleDepensePageQuery : IPageQuery<NouvelleDepensePage>
    {
    }

    public class NouvelleDepensePage : PageBase<NouvelleDepensePageQuery>
    {
        public AsyncCall<DepenseCreateDefaultValueQuery, DepenseCreateDefaultValueResponse> DefaultValue;
        public Select<PosteListQuery, PosteListResponseItem> Poste;
        public Form<DepenseCreateCommand, ListeDepensesPageQuery> Creer;

        public NouvelleDepensePage()
        {
            Init(this, x => x.DefaultValue.Result.Poste, x => x.Poste.Selected);
            Bind(this, x => x.Poste.Selected.Id, x => x.Creer.Command.IdPoste);
        }
    }

    public class NouvelleDepensePageModule : PageModule
    {
        public NouvelleDepensePageModule()
        {
            HandlePage<NouvelleDepensePageQuery, NouvelleDepensePage>((q, p) =>
            {
                p.Poste.Source.Query = new PosteListQuery();
            })
                .ForForm(p => p.Creer)
                .MapResponse<ID<Depense>>()
                .ToModel<ListeDepensesPageQuery>((c, r, m) => { })

                .ForAsyncCall(p => p.Poste.Source)
                .MapResponse<PosteListResponseItem[][]>()
                .ToSelf()

                .ForAsyncCall(p => p.DefaultValue)
                .MapResponse<DepenseCreateDefaultValueResponse>()
                .ToSelf();

        }
    }
}