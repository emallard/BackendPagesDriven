using System;
using CocoriCore;
using CocoriCore.Router;

namespace Comptes
{

    public class ComptesRouterConfiguration
    {

        public static void Load(RouterOptionsBuilder builder)
        {
            builder.SetPatternForType("[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}", typeof(ID<Poste>));
            builder.SetPatternForType("[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}", typeof(ID<Depense>));


            builder.Get<AccueilPageQuery>().SetPath("api/comptes");

            builder.Get<PosteListPageQuery>().SetPath("api/postes");
            builder.Get<PosteCreatePageQuery>().SetPath("api/postes/creer");
            builder.Get<PosteViewPageQuery>().SetPath(x => $"api/postes/{x.Id}");
            builder.Get<PosteUpdatePageQuery>().SetPath(x => $"api/postes/{x.Id}/modifier");

            builder.Get<DepenseListPageQuery>().SetPath("api/depenses");
            builder.Get<DepenseCreatePageQuery>().SetPath("api/depenses/creer");
            builder.Get<DepenseViewPageQuery>().SetPath(x => $"api/depenses/{x.Id}");
            builder.Get<DepenseUpdatePageQuery>().SetPath(x => $"api/depenses/{x.Id}/modifier");
        }
    }
}