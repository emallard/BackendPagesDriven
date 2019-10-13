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

            PageLogsRouterConfiguration.Load(builder);
            ComptesRouterConfiguration.Load(builder);
            builder.Post<GenericMessage>().SetPath("api/call").UseBody();

            var options = builder.Options;
            return builder.Options;
        }
    }
}