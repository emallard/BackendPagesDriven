using System;
using System.Linq;
using CocoriCore;
using CocoriCore.Router;
using Soltys.ChangeCase;

namespace CocoriCore
{
    public class RouteToUrl
    {
        private readonly RouterOptions routerOptions;

        public RouteToUrl(RouterOptions routerOptions)
        {
            this.routerOptions = routerOptions;
        }

        public string ToUrl(IMessage message)
        {
            var route = routerOptions.AllRoutes.First(r => r.MessageType == message.GetType());

            String url = route.ParameterizedUrl;
            foreach (var p in route.UrlParameters)
            {
                var i = url.IndexOf(p.Name.CamelCase() + ":");
                var j = url.IndexOf("/", i);
                url = url.Substring(0, i)
                    + p.InvokeGetter(message).ToString()
                    + (j == -1 ? "" : url.Substring(j, url.Length - j));
            }

            var query = "";
            var memberInfos = message.GetType().GetPropertiesAndFields();
            foreach (var mi in memberInfos)
            {
                if (!route.UrlParameters.Any(p => p.Name == mi.Name))
                {
                    if (query == "")
                        query += "?";
                    else
                        query += "&";
                    query += mi.Name + "=" + mi.InvokeGetter(message);
                }
            }

            return url + query;
        }
    }
}