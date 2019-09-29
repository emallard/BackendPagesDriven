using System;
using System.Threading.Tasks;
using CocoriCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Comptes;
using CocoriCore.Router;

namespace CocoriCore.PageLogs
{

    public class PageLogsRouterConfiguration
    {

        public static void Load(RouterOptionsBuilder builder)
        {
            builder.Get<HomePageQuery>().SetPath("api/pagelogs");
            builder.Get<PageListPageQuery>().SetPath("api/pagelogs/pages");
            builder.Get<TestListPageQuery>().SetPath("api/pagelogs/tests");

            builder.Get<PagePageQuery>().SetPath(x => $"api/pagelogs/page/{x.PageName}");
            builder.Get<EntityPageQuery>().SetPath(x => $"api/pagelogs/entity/{x.EntityName}");
            builder.Get<TestPageQuery>().SetPath(x => $"api/pagelogs/test/{x.TestName}");
            builder.Get<PageGraphPageQuery>().SetPath("api/pagelogs/pagegraph");
        }
    }
}