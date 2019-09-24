using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PageNouvelleDepenseQuery : IPageQuery<PageNouvelleDepense>
    {
    }

    public class PageNouvelleDepense : PageBase<PageNouvelleDepense>
    {
        public AsyncCall<DepenseCreateDefault> Modele;
        public Form<CreateCommand<DepenseCreate>, PageListeDepensesQuery> Creer;

        public PageNouvelleDepense()
        {
            //Bind(x => x.Modele.Result.Nom, x => x.Creer.Command.Object.Nom);
        }
    }

    public class PageNouvelleDepenseModule : PageModule
    {
        public PageNouvelleDepenseModule()
        {
            Handle<PageNouvelleDepenseQuery, PageNouvelleDepense>(q => new PageNouvelleDepense()
            {
                Modele = new AsyncCall<DepenseCreateDefault>(q),
                Creer = new Form<CreateCommand<DepenseCreate>, PageListeDepensesQuery>()
            });

            this.MapAsyncCall<PageNouvelleDepenseQuery, ByIdQuery<DepenseCreateDefault>, DepenseCreateDefault, DepenseCreateDefault>(
                pageQuery => new ByIdQuery<DepenseCreateDefault>(),
                (query, response) => response);

            this.MapForm<CreateCommand<DepenseCreate>, Guid, PageListeDepensesQuery>(
                (q, r) => new PageListeDepensesQuery()
            );
        }
    }
}