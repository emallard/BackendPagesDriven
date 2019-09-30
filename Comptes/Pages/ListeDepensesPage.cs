using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class ListeDepensesPageQuery : IPageQuery<ListeDepensesPage>
    {
    }

    public class ListeDepensesPage : PageBase<ListeDepensesPageQuery>
    {
        public NouvelleDepensePageQuery NouvelleDepense;
        public AsyncCall<DepenseListQuery, ListeDepensePageItem[]> Depenses;
    }

    public class ListeDepensePageItem
    {
        public DepensePageQuery Lien;
        public DepenseListResponseItem Depense;
    }

    public class ListeDepensesPageModule : PageModule
    {
        public ListeDepensesPageModule()
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
            this.HandlePage<ListeDepensesPageQuery, ListeDepensesPage>((q, p) => { })
                .ForAsyncCall(x => x.Depenses)
                .MapResponse<DepenseListResponseItem[]>()
                .ToModel<ListeDepensePageItem[]>((q, r) => r.Select(x => new ListeDepensePageItem()
                {
                    Lien = new DepensePageQuery() { Id = x.Id },
                    Depense = x
                }).ToArray());
        }
    }
}