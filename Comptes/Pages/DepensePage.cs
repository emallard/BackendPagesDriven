using System;
using CocoriCore;

namespace Comptes
{
    public class DepensePageQuery : IPageQuery<DepensePage>
    {
        public ID<Depense> Id;
    }

    public class DepensePage : PageBase<DepensePageQuery>
    {
        public AsyncCall<DepenseViewQuery, DepenseViewResponse> Depense;
    }

    public class DepensePageModule : PageModule
    {
        public DepensePageModule()
        {
            /*
            WhenAskPage<PageDepenseQuery>()
                .CreateQuery<ByIdQuery<DepenseView>>((p, q) => q.Id = p.Id)
                .ConstructModel<DepenseModel>((r, m) => m.Depense = r);
            */
            /*
            WhenAskPage<PageDepenseQuery>()
                .CallQuery<ByIdQuery<DepenseView>>((p, q) => q.Id = p.Id)
                .ThenConstructModel(m, r) => m.Depense = r)
                .CallQuery<ListQuery<Poste>>((p, q) => q),
                .ThenConstructModel(m, r) => m.Postes = r.Foreach(l => * add link *));
            */

            HandlePage<DepensePageQuery, DepensePage>((q, p) => { p.Depense.Query.Id = q.Id; })
                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseViewResponse>()
                .ToSelf();

        }
    }
}