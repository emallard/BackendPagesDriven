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

        public string ToParameterizedUrl(IMessage message)
        {
            var route = routerOptions.AllRoutes.FirstOrDefault(r => r.MessageType == message.GetType());
            if (route == null)
                throw new Exception("Did you forget to add route for " + message.GetType().FullName);

            return route.ParameterizedUrl;
        }
        public string ToParameterizedUrlShort(IMessage message)
        {
            return string.Join("/", ToParameterizedUrl(message).Split("/").Select(
                segment =>
                {
                    if (!segment.Contains(':'))
                        return segment;
                    return ":" + segment.Substring(0, segment.IndexOf(':'));
                }
            ));

        }

        public string ToUrl(IMessage message)
        {
            var route = routerOptions.AllRoutes.FirstOrDefault(r => r.MessageType == message.GetType());
            if (route == null)
                throw new Exception("Did you forget to add route for " + message.GetType().FullName);

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