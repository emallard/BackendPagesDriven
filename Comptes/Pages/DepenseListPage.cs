using System;
using System.Linq;
using CocoriCore;
using CocoriCore.Page;

namespace Comptes
{
    public class DepenseListPageQuery : IPageQuery<DepenseListPage>
    {
    }

    public class DepenseListPage : PageBase<DepenseListPageQuery>
    {
        public DepenseCreatePageQuery NouvelleDepense;
        public AsyncCall<DepenseListQuery, DepenseListPageItem[]> Depenses;
    }

    public class DepenseListPageItem
    {
        public PageLink<DepenseUpdatePageQuery> Modifier;
        public string NomPoste;
        public string Description;
        public double Montant;
        public DateTime Date;
    }

    public class DepenseListPageModule : PageModule
    {
        public DepenseListPageModule()
        {
            this.HandlePage<DepenseListPageQuery, DepenseListPage>((q, p) => { })
                .ForAsyncCall(x => x.Depenses)
                .MapResponse<DepenseListResponseItem[]>()
                .ToModel<DepenseListPageItem[]>((q, r) => r.Select(x => new DepenseListPageItem()
                {
                    Modifier = new PageLink<DepenseUpdatePageQuery>(new DepenseUpdatePageQuery() { Id = x.Id }, "Modifier"),
                    NomPoste = x.NomPoste,
                    Description = x.Description,
                    Montant = x.Montant,
                    Date = x.Date
                }).ToArray());
        }
    }
}