using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class PageNouvelleDepenseQuery : IPageQuery<PageNouvelleDepense>
    {
    }

    public class Select<TQuery, TModel> where TQuery : IMessage
    {
        public Select()
        {
            Source = new AsyncCall<TQuery, TModel[]>();
        }

        public AsyncCall<TQuery, TModel[]> Source;
        public TModel Selected;

        public void SetPageQuery(object pageQuery)
        {
            Source.SetPageQuery(pageQuery);
        }
    }

    public class PageNouvelleDepense : PageBase<PageNouvelleDepenseQuery>
    {
        public Select<ListQuery<PosteView>, PosteView> Poste;
        public Form<DepenseCreateCommand, PageListeDepensesQuery> Creer;

        public PageNouvelleDepense()
        {
            Bind(this, x => x.Poste.Selected.Id, x => x.Creer.Command.IdPoste);
        }
    }

    public class PageNouvelleDepenseModule : PageModule
    {
        public PageNouvelleDepenseModule()
        {
            HandlePage<PageNouvelleDepenseQuery, PageNouvelleDepense>()
                .ForForm(p => p.Creer)
                .MapResponse<ID<Depense>>()
                .ToModel<PageListeDepensesQuery>((c, r, m) => { })

                .ForAsyncCall(p => p.Poste.Source)
                .MapResponse<PosteView[]>()
                .ToSelf();
        }
    }
}