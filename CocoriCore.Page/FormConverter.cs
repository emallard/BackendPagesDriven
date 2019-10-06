using System;
using System.Linq;
using CocoriCore.Router;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Soltys.ChangeCase;

namespace CocoriCore
{


    public class FormConverter : JsonConverter
    {
        private readonly RouterOptions routerOptions;
        private readonly RouteToUrl routeToUrl;

        public FormConverter(RouterOptions routerOptions, RouteToUrl routeToUrl)
        {
            this.routerOptions = routerOptions;
            this.routeToUrl = routeToUrl;
        }

        public override bool CanWrite { get { return true; } }
        public override bool CanRead { get { return false; } }


        public override bool CanConvert(Type objectType)
        {
            return typeof(IForm).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new Exception("FormConverter must not be used to read");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jObject = new JObject();
            foreach (var mi in value.GetType().GetPropertiesAndFields())
            {
                var field = mi.InvokeGetter(value);
                var jfield = JToken.FromObject(field, serializer);
                jObject.Add(mi.Name, jfield);
            }


            var command = ((IForm)value).GetCommand();
            var jFieldTypeNames = new JObject();
            foreach (var mi in command.GetType().GetPropertiesAndFields())
            {
                jFieldTypeNames.Add(mi.Name, mi.GetMemberType().GetFriendlyName());
            }

            jObject.Add("CommandFieldTypeNames", jFieldTypeNames);
            jObject.WriteTo(writer);
        }
    }
}