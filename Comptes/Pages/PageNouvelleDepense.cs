using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PageNouvelleDepenseQuery : IPageQuery<PageNouvelleDepense>
    {
    }

    public class Select<T> : ISetPageQuery
    {
        public Select()
        {
            Source = new AsyncCall<T[]>();
        }

        public AsyncCall<T[]> Source;
        public T Selected;

        public void SetPageQuery(object pageQuery)
        {
            Source.SetPageQuery(pageQuery);
        }
    }

    public class PageNouvelleDepense : PageBase<PageNouvelleDepenseQuery, PageNouvelleDepense>
    {
        public Select<PosteView> Poste;
        public Form<CreateCommand<DepenseCreate>, PageListeDepensesQuery> Creer;

        public PageNouvelleDepense()
        {
            Bind(x => x.Poste.Selected.Id, x => x.Creer.Command.Object.IdPoste);
        }
    }

    public class PageNouvelleDepenseModule : PageModule
    {
        public PageNouvelleDepenseModule()
        {
            HandlePage<PageNouvelleDepenseQuery, PageNouvelleDepense>();

            this.MapAsyncCall<PageNouvelleDepenseQuery, ListQuery<PosteView>, PosteView[], PosteView[]>(
                pageQuery => new ListQuery<PosteView>(),
                (query, response) => response);

            this.MapForm<CreateCommand<DepenseCreate>, Guid, PageListeDepensesQuery>(
                (q, r) => new PageListeDepensesQuery()
            );
        }
    }
}