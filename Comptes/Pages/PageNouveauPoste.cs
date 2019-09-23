using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{
    public class PageBinding<TPage>
    {
        public Expression<Func<TPage, object>> From;
        public Expression<Func<TPage, object>> To;
    }

    public class PageBase<TPage>
    {
        public List<PageBinding<TPage>> Bindings = new List<PageBinding<TPage>>();
        public void Bind(Expression<Func<TPage, object>> from, Expression<Func<TPage, object>> to)
        {
            Bindings.Add(new PageBinding<TPage>()
            {
                From = from,
                To = to
            });
        }
    }

    public class PageNouveauPosteQuery : IMessage<PageNouveauPoste>
    {
    }

    public class PageNouveauPoste : PageBase<PageNouveauPoste>
    {
        public AsyncCall<PageNouveauPosteQuery, PosteCreateDefault> Modele;
        public Form<CreateCommand<PosteCreate>, PageListePostesQuery> Creer;

        public PageNouveauPoste()
        {
            Bind(x => x.Modele.Result.Nom, x => x.Creer.Command.Object.Nom);
        }
    }

    public class PageNouveauPosteModule : PageModule
    {
        public PageNouveauPosteModule()
        {
            Handle<PageNouveauPosteQuery, PageNouveauPoste>(q => new PageNouveauPoste()
            {
                Modele = new AsyncCall<PageNouveauPosteQuery, PosteCreateDefault>(),
                Creer = new Form<CreateCommand<PosteCreate>, PageListePostesQuery>()
            });
        }
    }
}