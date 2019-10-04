using System;
using System.Linq;
using CocoriCore;
using CocoriCore.Page;

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
        public PageLink Voir;
        public string NomPoste;
        public string Description;
        public double Montant;
    }

    public class DepenseListPageModule : PageModule
    {
        public DepenseListPageModule()
        {
            this.HandlePage<ListeDepensesPageQuery, DepenseListPage>((q, p) => { })
                .ForAsyncCall(x => x.Depenses)
                .MapResponse<DepenseListResponseItem[]>()
                .ToModel<DepenseListPageItem[]>((q, r) => r.Select(x => new DepenseListPageItem()
                {
                    Voir = new PageLink(new DepenseViewPageQuery() { Id = x.Id }, "Voir"),
                    NomPoste = x.NomPoste,
                    Description = x.Description,
                    Montant = x.Montant
                }).ToArray());
        }
    }
}