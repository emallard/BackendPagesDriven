using System;
using CocoriCore;

namespace Comptes
{
    public class DepenseViewPageQuery : IPageQuery<DepenseViewPage>
    {
        public ID<Depense> Id;
    }

    public class DepenseViewPage : PageBase<DepenseViewPageQuery>
    {
        public AsyncCall<DepenseViewQuery, DepenseViewResponse> Depense;
    }

    public class DepenseViewPageModule : PageModule
    {
        public DepenseViewPageModule()
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

            HandlePage<DepenseViewPageQuery, DepenseViewPage>((q, p) => { p.Depense.Query.Id = q.Id; })
                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseViewResponse>()
                .ToSelf();

        }
    }
}