using System;
using System.Linq;
using CocoriCore;
using CocoriCore.Router;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CocoriCore
{
    public class GenericMessageConverter : JsonConverter
    {
        public GenericMessageConverter()
        {
        }

        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            return typeof(GenericMessage).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var call = jObject.ToObject<GenericMessage>();
            var newObj = jObject.ToObject(call._Type, serializer);
            return newObj;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }

}