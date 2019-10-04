using System;
using CocoriCore;

namespace Comptes
{
    public class DepenseViewPageQuery : IPageQuery<DepenseViewPage>
    {
        public ID<Depense> Id;
    }

    public class DepenseViewPage : PageBase<DepenseViewPageQuery>
    {
        public AsyncCall<DepenseViewQuery, DepenseViewResponse> Depense;
    }

    public class DepenseViewPageModule : PageModule
    {
        public DepenseViewPageModule()
        {
            HandlePage<DepenseViewPageQuery, DepenseViewPage>((q, p) => { p.Depense.Query.Id = q.Id; })
                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseViewResponse>()
                .ToSelf();

        }
    }
}