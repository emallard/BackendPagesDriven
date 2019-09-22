using System;
using System.Linq;
using CocoriCore.Router;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Soltys.ChangeCase;

namespace CocoriCore
{


    public class PageConverter : JsonConverter
    {
        private readonly RouterOptions routerOptions;
        private readonly RouteToUrl routeToUrl;

        public PageConverter(RouterOptions routerOptions, RouteToUrl routeToUrl)
        {
            this.routerOptions = routerOptions;
            this.routeToUrl = routeToUrl;
        }

        public override bool CanWrite { get { return true; } }
        public override bool CanRead { get { return false; } }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IPageQuery).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new Exception("IPage Not supposed to be deserialized with PageConverter");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var message = (IMessage)value;
            var url = this.routeToUrl.ToUrl(message);
            JObject jObject = JObject.FromObject(value);
            jObject.Add("href", JToken.FromObject(url));
            jObject.WriteTo(writer);
        }
    }
}