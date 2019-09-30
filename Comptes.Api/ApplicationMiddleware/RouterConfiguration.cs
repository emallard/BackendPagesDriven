using System;
using System.Threading.Tasks;
using CocoriCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Comptes;
using CocoriCore.Router;
using CocoriCore.PageLogs;

namespace Comptes.Api
{

    public class RouterConfiguration
    {

        public static RouterOptions Options()
        {
            var builder = new RouterOptionsBuilder();

            builder.SetPatternForType("[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}", typeof(ID<Poste>));
            builder.SetPatternForType("[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}", typeof(ID<Depense>));


            builder.Get<AccueilPageQuery>().SetPath("api");

            builder.Get<PageListePostesQuery>().SetPath("api/postes");
            builder.Get<NouveauPostePageQuery>().SetPath("api/postes/creer");
            builder.Get<PostePageQuery>().SetPath(x => $"api/postes/{x.Id}");

            builder.Get<ListeDepensesPageQuery>().SetPath("api/depenses");
            builder.Get<NouvelleDepensePageQuery>().SetPath("api/depenses/creer");
            builder.Get<DepensePageQuery>().SetPath(x => $"api/depenses/{x.Id}");

            builder.Post<Call>().SetPath("api/call").UseBody();
            builder.Get<HtmlMessage>().SetPath("api/page").UseQuery();
            builder.Get<FavIconMessage>().SetPath("favicon.ico").UseQuery();
            builder.Get<Tests_GET>().SetPath("api/tests");
            builder.Get<Tests_Id_GET>().SetPath(x => $"api/tests/{x.Type}/{x.TestName}").UseQuery();

            builder.Get<Graph_GET>().SetPath("api/graph").UseQuery();

            PageLogsRouterConfiguration.Load(builder);

            var options = builder.Options;
            return builder.Options;
        }
    }
}