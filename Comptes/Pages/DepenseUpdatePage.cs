using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CocoriCore;

namespace Comptes
{

    public class DepenseUpdatePageQuery : IPageQuery<DepenseUpdatePage>
    {
        public ID<Depense> Id;
    }

    public class DepenseUpdatePage : PageBase<DepenseUpdatePageQuery>
    {
        public AsyncCall<DepenseUpdateInitQuery, DepenseUpdateInitResponse> Depense;
        public Select<PosteListQuery, ValueLabel<ID<Poste>>> PosteSelect;

        public Form<DepenseUpdateCommand, DepenseListPageQuery> Enregistrer;

        public DepenseUpdatePage()
        {
            OnInit(this, x => x.PageQuery.Id, x => x.Enregistrer.Command.Id);
            OnInit(this, x => x.Depense.Result.Description, x => x.Enregistrer.Command.Description);
            OnInit(this, x => x.Depense.Result.Montant, x => x.Enregistrer.Command.Montant);
            OnInit(this, x => x.Depense.Result.Date, x => x.Enregistrer.Command.Date);
            OnInit(this, x => x.Depense.Result.Poste, x => x.PosteSelect.Selected);

            OnSubmit(this, x => x.PosteSelect.Selected.Value, x => x.Enregistrer.Command.IdPoste);
        }
    }

    public class DepenseUpdatePageModule : PageModule
    {
        public DepenseUpdatePageModule()
        {
            HandlePage<DepenseUpdatePageQuery, DepenseUpdatePage>((q, p) =>
            {
                p.Depense.Query.Id = q.Id;
            })
                .ForForm(p => p.Enregistrer)
                .MapResponse<CocoriCore.Empty>()
                .ToModel<DepenseListPageQuery>((q, r, m) => { });

        }
    }
}