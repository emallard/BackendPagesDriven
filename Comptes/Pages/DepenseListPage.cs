using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class ListeDepensesPageQuery : IPageQuery<DepenseListPage>
    {
    }

    public class DepenseListPage : PageBase<ListeDepensesPageQuery>
    {
        public DepenseCreatePageQuery NouvelleDepense;
        public AsyncCall<DepenseListQuery, DepenseListPageItem[]> Depenses;
    }

    public class DepenseListPageItem
    {
        public DepenseViewPageQuery Lien;
        public DepenseListResponseItem Depense;
    }

    public class DepenseListPageModule : PageModule
    {
        public DepenseListPageModule()
        {
            /*
            this.MapAsyncCall<PageListeDepensesQuery, ListQuery<DepenseView>, DepenseView[], PageListeDepenseItem[]>(
                q => new ListQuery<DepenseView>(),
                (q, r) => r.Select(x => new PageListeDepenseItem()
                {
                    Lien = new PageDepenseQuery { Id = x.Id.Id },
                    Depense = x
                }).ToArray()
                );
            */
            this.HandlePage<ListeDepensesPageQuery, DepenseListPage>((q, p) => { })
                .ForAsyncCall(x => x.Depenses)
                .MapResponse<DepenseListResponseItem[]>()
                .ToModel<DepenseListPageItem[]>((q, r) => r.Select(x => new DepenseListPageItem()
                {
                    Lien = new DepenseViewPageQuery() { Id = x.Id },
                    Depense = x
                }).ToArray());
        }
    }
}