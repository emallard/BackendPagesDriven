using System;
using CocoriCore;

namespace Comptes
{
    public class PageDepenseQuery : IPageQuery<PageDepense>
    {
        public Guid Id;
    }

    public class PageDepense : PageBase<PageDepenseQuery>
    {
        public AsyncCall<ByIdQuery<DepenseView>, DepenseView> Depense;
    }

    public class PageDepenseModule : PageModule
    {
        public PageDepenseModule()
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

            HandlePage<PageDepenseQuery, PageDepense>((q, p) => { p.Depense.Query.Id = q.Id; })
                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseView>()
                .ToSelf();

            /*
            this.Map<PageDepenseQuery, ByIdQuery<DepenseView>>(p => new ByIdQuery<DepenseView>() { Id = p.Id });
            this.Map<ByIdQuery<DepenseView>, DepenseView, DepenseView>((q, r) => r);
            
            this.MapAsyncCall<PageDepenseQuery, ByIdQuery<DepenseView>, DepenseView, DepenseView>(
                p => new ByIdQuery<DepenseView>() { Id = p.Id },
                (q, r) => r
            );*/


        }
    }
}