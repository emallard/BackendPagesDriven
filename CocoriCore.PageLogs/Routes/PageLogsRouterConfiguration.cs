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
            builder.Get<PagePageQuery>().SetPath(x => $"api/pagelogs/page/{x.PageName}");
        }
    }
}