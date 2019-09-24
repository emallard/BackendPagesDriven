using System;
using System.Linq;
using CocoriCore;

namespace Comptes
{
    public class PageListeDepensesQuery : IPageQuery<PageListeDepenses>
    {
    }

    public class PageListeDepenses
    {
        public PageNouvelleDepenseQuery NouvelleDepense;
        public AsyncCall<PageListeDepenseItem[]> Depenses;
    }

    public class PageListeDepenseItem
    {
        public PageDepenseQuery Lien;
        public DepenseView Depense;
    }

    public class PageListeDepensesModule : PageModule
    {
        public PageListeDepensesModule()
        {
            this.MapAsyncCall<PageListeDepensesQuery, ListQuery<DepenseView>, DepenseView[], PageListeDepenseItem[]>(
                q => new ListQuery<DepenseView>(),
                (q, r) => r.Select(x => new PageListeDepenseItem()
                {
                    Lien = new PageDepenseQuery { Id = x.Id.Id },
                    Depense = x
                }).ToArray()
                );
            this.Handle<PageListeDepensesQuery, PageListeDepenses>(q => new PageListeDepenses()
            {
                Depenses = new AsyncCall<PageListeDepenseItem[]>(q),
                NouvelleDepense = new PageNouvelleDepenseQuery()
            });
        }
    }
}