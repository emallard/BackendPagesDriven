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
        public OnInitCall<DepenseViewQuery, DepenseViewResponse> Depense;
    }

    public class DepenseViewPageModule : PageModule
    {
        public DepenseViewPageModule()
        {
            HandlePage<DepenseViewPageQuery, DepenseViewPage>((q, p) => { p.Depense.Message.Id = q.Id; })
                .ForAsyncCall(p => p.Depense)
                .MapResponse<DepenseViewResponse>()
                .ToSelf();

        }
    }
}